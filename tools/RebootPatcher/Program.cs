using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace RebootPatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string targetDll = Path.GetFullPath(Path.Combine("deploy", "AbsolutDashboard.Web.dll"));
            string portableDll = Path.GetFullPath(Path.Combine("AbsolutPatchPortable", "AbsolutDashboard.Web.dll"));
            string moduleDll = Path.GetFullPath(Path.Combine("tools", "RebootModule", "bin", "Release", "net10.0", "RebootModule.dll"));

            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(Path.GetDirectoryName(targetDll)!);
            resolver.AddSearchDirectory(Path.GetDirectoryName(moduleDll)!);
            string dotnetRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "dotnet", "shared");
            foreach (var fw in new[] { "Microsoft.AspNetCore.App", "Microsoft.NETCore.App" })
            {
                string fwDir = Path.Combine(dotnetRoot, fw);
                if (Directory.Exists(fwDir))
                {
                    foreach (var verDir in Directory.GetDirectories(fwDir))
                        resolver.AddSearchDirectory(verDir);
                }
            }

            using var targetAsm = AssemblyDefinition.ReadAssembly(targetDll, new ReaderParameters { ReadWrite = false, AssemblyResolver = resolver });
            using var modAsm = AssemblyDefinition.ReadAssembly(moduleDll, new ReaderParameters { AssemblyResolver = resolver });

            var mainMod = targetAsm.MainModule;

            // 1. Make OperationRunner.RunStepAsync public so RebootSystem can call it if needed
            var opRunner = mainMod.Types.First(t => t.FullName == "AbsolutDashboard.Web.Services.Operations.OperationRunner");
            var runStep = opRunner.Methods.First(m => m.Name == "RunStepAsync");
            runStep.IsPublic = true;

            // 2. Copy RebootOptions and RebootSystem (and nested types) into targetAsm
            var typeMap = new Dictionary<TypeDefinition, TypeDefinition>();
            var typesToCopy = modAsm.MainModule.Types
                .Where(t => t.Namespace == "AbsolutDashboard.Web.Services.Operations" || t.Namespace == "AbsolutDashboard.Web.Components")
                .ToList();

            foreach (var srcType in typesToCopy)
            {
                var cloned = CloneTypeShell(srcType, mainMod, typeMap);
                mainMod.Types.Add(cloned);
            }
            var fieldMap = new Dictionary<FieldDefinition, FieldDefinition>();
            var methodMap = new Dictionary<MethodDefinition, MethodDefinition>();
            foreach (var srcType in typesToCopy)
            {
                PopulateTypeMembers(srcType, typeMap[srcType], mainMod, typeMap, fieldMap, methodMap);
            }
            foreach (var srcType in typesToCopy)
            {
                PopulateTypeBodies(srcType, mainMod, typeMap, fieldMap, methodMap);
            }

            var rebootOptionsType = typeMap.Values.First(t => t.Name == "RebootOptions");
            var rebootSystemType = typeMap.Values.First(t => t.Name == "RebootSystem");

            // 3. Add Reboot property to OperationsOptions
            var opsOptions = mainMod.Types.First(t => t.FullName == "AbsolutDashboard.Web.Services.Operations.OperationsOptions");
            if (!opsOptions.Properties.Any(p => p.Name == "Reboot"))
            {
                var backingField = new FieldDefinition("<Reboot>k__BackingField", FieldAttributes.Private, rebootOptionsType);
                opsOptions.Fields.Add(backingField);

                var getter = new MethodDefinition("get_Reboot", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, rebootOptionsType);
                var gIl = getter.Body.GetILProcessor();
                gIl.Append(gIl.Create(OpCodes.Ldarg_0));
                gIl.Append(gIl.Create(OpCodes.Ldfld, backingField));
                gIl.Append(gIl.Create(OpCodes.Ret));
                opsOptions.Methods.Add(getter);

                var setter = new MethodDefinition("set_Reboot", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, mainMod.TypeSystem.Void);
                setter.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, rebootOptionsType));
                var sIl = setter.Body.GetILProcessor();
                sIl.Append(sIl.Create(OpCodes.Ldarg_0));
                sIl.Append(sIl.Create(OpCodes.Ldarg_1));
                sIl.Append(sIl.Create(OpCodes.Stfld, backingField));
                sIl.Append(sIl.Create(OpCodes.Ret));
                opsOptions.Methods.Add(setter);

                var prop = new PropertyDefinition("Reboot", PropertyAttributes.None, rebootOptionsType) { GetMethod = getter, SetMethod = setter };
                opsOptions.Properties.Add(prop);

                // Initialize in .ctor
                var ctor = opsOptions.Methods.First(m => m.IsConstructor && !m.IsStatic);
                var cIl = ctor.Body.GetILProcessor();
                var rebootCtor = rebootOptionsType.Methods.First(m => m.IsConstructor && !m.IsStatic);
                var ret = ctor.Body.Instructions.Last();
                cIl.InsertBefore(ret, cIl.Create(OpCodes.Ldarg_0));
                cIl.InsertBefore(ret, cIl.Create(OpCodes.Newobj, rebootCtor));
                cIl.InsertBefore(ret, cIl.Create(OpCodes.Stfld, backingField));
            }

            // 4. Add Reboot property to OperationRunner
            if (!opRunner.Properties.Any(p => p.Name == "Reboot"))
            {
                var optsField = opRunner.Fields.First(f => f.Name == "_opts");
                var getReboot = new MethodDefinition("get_Reboot", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, rebootOptionsType);
                var il = getReboot.Body.GetILProcessor();
                il.Append(il.Create(OpCodes.Ldarg_0));
                il.Append(il.Create(OpCodes.Ldfld, optsField));
                il.Append(il.Create(OpCodes.Callvirt, opsOptions.Properties.First(p => p.Name == "Reboot").GetMethod));
                il.Append(il.Create(OpCodes.Ret));
                opRunner.Methods.Add(getReboot);

                var prop = new PropertyDefinition("Reboot", PropertyAttributes.None, rebootOptionsType) { GetMethod = getReboot };
                opRunner.Properties.Add(prop);
            }

            // 5. Patch Home's AuthorizeView ChildContent lambda to render <div class="grid gap-4 lg:grid-cols-2"><ShutdownAll /><RebootSystem /></div>
            var homeType = mainMod.Types.First(t => t.FullName == "AbsolutDashboard.Web.Components.Pages.Home");
            var shutdownAllType = mainMod.Types.First(t => t.FullName == "AbsolutDashboard.Web.Components.ShutdownAll");

            MethodDefinition? childContentMethod = null;
            foreach (var nt in homeType.NestedTypes)
            {
                foreach (var m in nt.Methods)
                {
                    if (m.HasBody && m.Body.Instructions.Any(i => i.OpCode == OpCodes.Callvirt && i.Operand is GenericInstanceMethod gm && gm.GenericArguments.Any(ga => ga.FullName == shutdownAllType.FullName)))
                    {
                        childContentMethod = m;
                        break;
                    }
                }
                if (childContentMethod != null) break;
            }

            if (childContentMethod == null)
            {
                // Fallback: search Home methods directly
                foreach (var m in homeType.Methods)
                {
                    if (m.HasBody && m.Body.Instructions.Any(i => i.OpCode == OpCodes.Callvirt && i.Operand is GenericInstanceMethod gm && gm.GenericArguments.Any(ga => ga.FullName == shutdownAllType.FullName)))
                    {
                        childContentMethod = m;
                        break;
                    }
                }
            }

            if (childContentMethod == null)
                throw new Exception("Could not find Home AuthorizeView ChildContent method.");

            var rtbType = childContentMethod.Parameters[0].ParameterType.Resolve();
            var openElem = mainMod.ImportReference(rtbType.Methods.First(m => m.Name == "OpenElement" && m.Parameters.Count == 2));
            var addAttr = mainMod.ImportReference(rtbType.Methods.First(m => m.Name == "AddAttribute" && m.Parameters.Count == 3 && m.Parameters[2].ParameterType.FullName == "System.String"));
            var closeElem = mainMod.ImportReference(rtbType.Methods.First(m => m.Name == "CloseElement"));
            var closeComp = mainMod.ImportReference(rtbType.Methods.First(m => m.Name == "CloseComponent"));
            var openCompDef = rtbType.Methods.First(m => m.Name == "OpenComponent" && m.HasGenericParameters);

            var openShutdown = new GenericInstanceMethod(mainMod.ImportReference(openCompDef));
            openShutdown.GenericArguments.Add(shutdownAllType);

            var openReboot = new GenericInstanceMethod(mainMod.ImportReference(openCompDef));
            openReboot.GenericArguments.Add(rebootSystemType);

            var hIl = childContentMethod.Body.GetILProcessor();
            childContentMethod.Body.Instructions.Clear();
            childContentMethod.Body.Variables.Clear();
            childContentMethod.Body.ExceptionHandlers.Clear();

            // __builder2.OpenElement(8, "div");
            hIl.Append(hIl.Create(OpCodes.Ldarg_1));
            hIl.Append(hIl.Create(OpCodes.Ldc_I4_8));
            hIl.Append(hIl.Create(OpCodes.Ldstr, "div"));
            hIl.Append(hIl.Create(OpCodes.Callvirt, openElem));
            // __builder2.AddAttribute(9, "class", "grid gap-4 lg:grid-cols-2");
            hIl.Append(hIl.Create(OpCodes.Ldarg_1));
            hIl.Append(hIl.Create(OpCodes.Ldc_I4_S, (sbyte)9));
            hIl.Append(hIl.Create(OpCodes.Ldstr, "class"));
            hIl.Append(hIl.Create(OpCodes.Ldstr, "grid gap-4 lg:grid-cols-2"));
            hIl.Append(hIl.Create(OpCodes.Callvirt, addAttr));
            // __builder2.OpenComponent<ShutdownAll>(10); __builder2.CloseComponent();
            hIl.Append(hIl.Create(OpCodes.Ldarg_1));
            hIl.Append(hIl.Create(OpCodes.Ldc_I4_S, (sbyte)10));
            hIl.Append(hIl.Create(OpCodes.Callvirt, openShutdown));
            hIl.Append(hIl.Create(OpCodes.Ldarg_1));
            hIl.Append(hIl.Create(OpCodes.Callvirt, closeComp));
            // __builder2.OpenComponent<RebootSystem>(11); __builder2.CloseComponent();
            hIl.Append(hIl.Create(OpCodes.Ldarg_1));
            hIl.Append(hIl.Create(OpCodes.Ldc_I4_S, (sbyte)11));
            hIl.Append(hIl.Create(OpCodes.Callvirt, openReboot));
            hIl.Append(hIl.Create(OpCodes.Ldarg_1));
            hIl.Append(hIl.Create(OpCodes.Callvirt, closeComp));
            // __builder2.CloseElement();
            hIl.Append(hIl.Create(OpCodes.Ldarg_1));
            hIl.Append(hIl.Create(OpCodes.Callvirt, closeElem));
            hIl.Append(hIl.Create(OpCodes.Ret));

            string tempOut = targetDll + ".patched";
            targetAsm.Write(tempOut);
            targetAsm.Dispose();
            modAsm.Dispose();
            resolver.Dispose();
            File.Copy(tempOut, targetDll, true);
            File.Copy(tempOut, portableDll, true);
            File.Delete(tempOut);

            Console.WriteLine("Successfully patched deploy/AbsolutDashboard.Web.dll and AbsolutPatchPortable/AbsolutDashboard.Web.dll");
        }

        static TypeDefinition CloneTypeShell(TypeDefinition src, ModuleDefinition targetMod, Dictionary<TypeDefinition, TypeDefinition> map)
        {
            var t = new TypeDefinition(src.Namespace, src.Name, src.Attributes);
            map[src] = t;
            foreach (var gp in src.GenericParameters)
                t.GenericParameters.Add(new GenericParameter(gp.Name, t));
            foreach (var nt in src.NestedTypes)
                t.NestedTypes.Add(CloneTypeShell(nt, targetMod, map));
            return t;
        }

        static TypeReference ImportRef(TypeReference tr, ModuleDefinition targetMod, Dictionary<TypeDefinition, TypeDefinition> map)
        {
            if (tr is TypeDefinition td && map.TryGetValue(td, out var mapped))
                return mapped;
            if (tr is GenericInstanceType git)
            {
                var ngit = new GenericInstanceType(ImportRef(git.ElementType, targetMod, map));
                foreach (var ga in git.GenericArguments)
                    ngit.GenericArguments.Add(ImportRef(ga, targetMod, map));
                return ngit;
            }
            if (tr is ArrayType at)
                return new ArrayType(ImportRef(at.ElementType, targetMod, map), at.Rank);
            if (tr is ByReferenceType brt)
                return new ByReferenceType(ImportRef(brt.ElementType, targetMod, map));
            if (tr is GenericParameter gp)
                return gp;
            return targetMod.ImportReference(tr);
        }

        static void PopulateTypeMembers(TypeDefinition src, TypeDefinition dst, ModuleDefinition targetMod, Dictionary<TypeDefinition, TypeDefinition> map, Dictionary<FieldDefinition, FieldDefinition> fieldMap, Dictionary<MethodDefinition, MethodDefinition> methodMap)
        {
            if (src.BaseType != null)
                dst.BaseType = ImportRef(src.BaseType, targetMod, map);
            foreach (var iface in src.Interfaces)
                dst.Interfaces.Add(new InterfaceImplementation(ImportRef(iface.InterfaceType, targetMod, map)));

            foreach (var f in src.Fields)
            {
                var nf = new FieldDefinition(f.Name, f.Attributes, ImportRef(f.FieldType, targetMod, map));
                dst.Fields.Add(nf);
                fieldMap[f] = nf;
            }

            foreach (var m in src.Methods)
            {
                var nm = new MethodDefinition(m.Name, m.Attributes, ImportRef(m.ReturnType, targetMod, map));
                foreach (var gp in m.GenericParameters)
                    nm.GenericParameters.Add(new GenericParameter(gp.Name, nm));
                foreach (var p in m.Parameters)
                    nm.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, ImportRef(p.ParameterType, targetMod, map)));
                foreach (var ca in m.CustomAttributes)
                    nm.CustomAttributes.Add(new CustomAttribute(targetMod.ImportReference(ca.Constructor)));
                dst.Methods.Add(nm);
                methodMap[m] = nm;
            }

            foreach (var p in src.Properties)
            {
                var np = new PropertyDefinition(p.Name, p.Attributes, ImportRef(p.PropertyType, targetMod, map));
                if (p.GetMethod != null) np.GetMethod = methodMap[p.GetMethod];
                if (p.SetMethod != null) np.SetMethod = methodMap[p.SetMethod];
                foreach (var ca in p.CustomAttributes)
                    np.CustomAttributes.Add(new CustomAttribute(targetMod.ImportReference(ca.Constructor)));
                dst.Properties.Add(np);
            }

            foreach (var nt in src.NestedTypes)
                PopulateTypeMembers(nt, map[nt], targetMod, map, fieldMap, methodMap);
        }

        static void PopulateTypeBodies(TypeDefinition src, ModuleDefinition targetMod, Dictionary<TypeDefinition, TypeDefinition> map, Dictionary<FieldDefinition, FieldDefinition> fieldMap, Dictionary<MethodDefinition, MethodDefinition> methodMap)
        {

            foreach (var m in src.Methods)
            {
                if (!m.HasBody) continue;
                m.Body.SimplifyMacros();
                var nm = methodMap[m];
                nm.Body.InitLocals = m.Body.InitLocals;
                nm.Body.MaxStackSize = m.Body.MaxStackSize;
                foreach (var v in m.Body.Variables)
                    nm.Body.Variables.Add(new VariableDefinition(ImportRef(v.VariableType, targetMod, map)));

                var il = nm.Body.GetILProcessor();
                var instrMap = new Dictionary<Instruction, Instruction>();
                foreach (var instr in m.Body.Instructions)
                {
                    Instruction ni;
                    if (instr.Operand == null) ni = il.Create(instr.OpCode);
                    else if (instr.Operand is sbyte sb) ni = il.Create(instr.OpCode, sb);
                    else if (instr.Operand is byte b) ni = il.Create(instr.OpCode, b);
                    else if (instr.Operand is int i32) ni = il.Create(instr.OpCode, i32);
                    else if (instr.Operand is long i64) ni = il.Create(instr.OpCode, i64);
                    else if (instr.Operand is float f32) ni = il.Create(instr.OpCode, f32);
                    else if (instr.Operand is double f64) ni = il.Create(instr.OpCode, f64);
                    else if (instr.Operand is string s) ni = il.Create(instr.OpCode, s);
                    else if (instr.Operand is TypeReference tr) ni = il.Create(instr.OpCode, ImportRef(tr, targetMod, map));
                    else if (instr.Operand is FieldReference fr)
                    {
                        var rfd = fr.Resolve();
                        if (rfd != null && fieldMap.TryGetValue(rfd, out var mf)) ni = il.Create(instr.OpCode, mf);
                        else ni = il.Create(instr.OpCode, targetMod.ImportReference(fr));
                    }
                    else if (instr.Operand is MethodReference mr)
                    {
                        var rmd = mr.Resolve();
                        if (rmd != null && methodMap.TryGetValue(rmd, out var mm)) ni = il.Create(instr.OpCode, mm);
                        else ni = il.Create(instr.OpCode, targetMod.ImportReference(mr));
                    }
                    else if (instr.Operand is ParameterDefinition pd) ni = il.Create(instr.OpCode, pd.Index == -1 ? nm.Body.ThisParameter : nm.Parameters[pd.Index]);
                    else if (instr.Operand is VariableDefinition vd) ni = il.Create(instr.OpCode, nm.Body.Variables[vd.Index]);
                    else if (instr.Operand is Instruction) ni = il.Create(OpCodes.Nop);
                    else if (instr.Operand is Instruction[]) ni = il.Create(OpCodes.Nop);
                    else ni = il.Create(instr.OpCode);

                    instrMap[instr] = ni;
                    il.Append(ni);
                }
                foreach (var instr in m.Body.Instructions)
                {
                    if (instr.Operand is Instruction targetInstr)
                        instrMap[instr].Operand = instrMap[targetInstr];
                    else if (instr.Operand is Instruction[] targets)
                        instrMap[instr].Operand = targets.Select(ti => instrMap[ti]).ToArray();
                }
                foreach (var eh in m.Body.ExceptionHandlers)
                {
                    var neh = new ExceptionHandler(eh.HandlerType)
                    {
                        TryStart = instrMap[eh.TryStart],
                        TryEnd = instrMap[eh.TryEnd],
                        HandlerStart = instrMap[eh.HandlerStart],
                        HandlerEnd = instrMap[eh.HandlerEnd],
                        FilterStart = eh.FilterStart != null ? instrMap[eh.FilterStart] : null,
                        CatchType = eh.CatchType != null ? ImportRef(eh.CatchType, targetMod, map) : null
                    };
                    nm.Body.ExceptionHandlers.Add(neh);
                }
                nm.Body.OptimizeMacros();
            }

            foreach (var nt in src.NestedTypes)
                PopulateTypeBodies(nt, targetMod, map, fieldMap, methodMap);
        }
    }
}
