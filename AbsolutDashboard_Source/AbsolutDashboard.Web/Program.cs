using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AbsolutDashboard.Web;
using AbsolutDashboard.Web.Components;
using AbsolutDashboard.Web.Components.Account;
using AbsolutDashboard.Web.Data;
using AbsolutDashboard.Web.Data.Entities;
using AbsolutDashboard.Web.Services.Collectors;
using AbsolutDashboard.Web.Services.Collectors.Bbu;
using AbsolutDashboard.Web.Services.Collectors.Core;
using AbsolutDashboard.Web.Services.Collectors.Rhub;
using AbsolutDashboard.Web.Services.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Token: 0x02000016 RID: 22
public class Program
{
	// Token: 0x060000B9 RID: 185 RVA: 0x00005174 File Offset: 0x00003374
	private static async Task <Main>$(string[] args)
	{
		Program.<>c__DisplayClass0_0 CS$<>8__locals1 = new Program.<>c__DisplayClass0_0();
		WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
		webApplicationBuilder.Services.AddRazorComponents(null).AddInteractiveServerComponents(null);
		webApplicationBuilder.Services.AddAuthorization();
		webApplicationBuilder.Services.AddCascadingAuthenticationState();
		webApplicationBuilder.Services.AddScoped<IdentityUserAccessor>();
		webApplicationBuilder.Services.AddScoped<IdentityRedirectManager>();
		webApplicationBuilder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
		webApplicationBuilder.Services.AddAuthentication(delegate(AuthenticationOptions options)
		{
			options.DefaultScheme = IdentityConstants.ApplicationScheme;
			options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
		}).AddIdentityCookies();
		Program.<>c__DisplayClass0_0 CS$<>8__locals2 = CS$<>8__locals1;
		string connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection");
		if (connectionString == null)
		{
			throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		}
		CS$<>8__locals2.connectionString = connectionString;
		webApplicationBuilder.Services.AddDbContextFactory<ApplicationDbContext>(delegate(DbContextOptionsBuilder options)
		{
			options.UseSqlite(CS$<>8__locals1.connectionString, null);
		}, ServiceLifetime.Singleton);
		webApplicationBuilder.Services.AddScoped<ApplicationDbContext>((IServiceProvider sp) => sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
		webApplicationBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();
		webApplicationBuilder.Services.AddIdentityCore<ApplicationUser>(delegate(IdentityOptions options)
		{
			options.SignIn.RequireConfirmedAccount = false;
		}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
			.AddSignInManager()
			.AddDefaultTokenProviders();
		webApplicationBuilder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
		webApplicationBuilder.Services.AddDataProtection().SetApplicationName("AbsolutDashboard");
		string text = webApplicationBuilder.Configuration["DataProtection:KeyDirectory"];
		if (!string.IsNullOrWhiteSpace(text))
		{
			Directory.CreateDirectory(text);
			webApplicationBuilder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(text)).SetApplicationName("AbsolutDashboard");
		}
		webApplicationBuilder.Services.AddSingleton<CredentialProtector>();
		webApplicationBuilder.Services.AddSingleton<ISshOutputParser, GenericSshParser>();
		webApplicationBuilder.Services.AddSingleton<ISshOutputParser, BtiAuParser>();
		webApplicationBuilder.Services.AddSingleton<ISshOutputParser, H3c5gcParser>();
		webApplicationBuilder.Services.AddSingleton<SshParserRegistry>();
		webApplicationBuilder.Services.AddSingleton<IDeviceCollector, SnmpCollector>();
		webApplicationBuilder.Services.AddSingleton<IDeviceCollector, SshScriptCollector>();
		webApplicationBuilder.Services.AddSingleton<IDeviceCollector, BbuMmlCollector>();
		webApplicationBuilder.Services.AddSingleton<RhubSerialClient>();
		webApplicationBuilder.Services.AddSingleton<BbuSessionLock>();
		webApplicationBuilder.Services.AddSingleton<BbuConfigWriter>();
		webApplicationBuilder.Services.AddSingleton<CoreN6Writer>();
		webApplicationBuilder.Services.Configure<PollingOptions>(webApplicationBuilder.Configuration.GetSection("Polling"));
		webApplicationBuilder.Services.Configure<OperationsOptions>(webApplicationBuilder.Configuration.GetSection("Operations"));
		webApplicationBuilder.Services.AddSingleton<OperationRunner>();
		webApplicationBuilder.Services.AddHostedService<DevicePollingService>();
		webApplicationBuilder.Services.AddSingleton<CoreLiveState>();
		webApplicationBuilder.Services.AddHostedService<CoreLivePollService>();
		webApplicationBuilder.Services.Configure<ForwardedHeadersOptions>(delegate(ForwardedHeadersOptions options)
		{
			options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			options.KnownNetworks.Clear();
			options.KnownProxies.Clear();
		});
		WebApplication app = webApplicationBuilder.Build();
		app.Logger.LogInformation("AbsolutDashboard {Version} starting.", new object[] { AppVersion.Full });
		using (IServiceScope scope = app.Services.CreateScope())
		{
			ApplicationDbContext requiredService = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			requiredService.Database.Migrate();
		}
		IServiceScope scope = null;
		await DbSeeder.SeedAsync(app.Services, app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder"));
		string resetPassword = app.Configuration["Setup:ResetPassword:Password"];
		if (!string.IsNullOrWhiteSpace(resetPassword))
		{
			string text2 = app.Configuration["Setup:ResetPassword:Email"];
			string resetEmail = ((text2 != null) ? text2.Trim() : null);
			ILogger setupLog = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Setup");
			UserManager<ApplicationUser> userManager;
			ApplicationUser target;
			using (IServiceScope scope = app.Services.CreateScope())
			{
				userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				ApplicationUser applicationUser;
				if (string.IsNullOrWhiteSpace(resetEmail))
				{
					IQueryable<ApplicationUser> users = userManager.Users;
					ParameterExpression parameterExpression = Expression.Parameter(typeof(ApplicationUser), "u");
					applicationUser = users.FirstOrDefault<ApplicationUser>(Expression.Lambda<Func<ApplicationUser, bool>>(Expression.Property(parameterExpression, methodof(ApplicationUser.get_IsOwner())), new ParameterExpression[] { parameterExpression }));
				}
				else
				{
					applicationUser = await userManager.FindByEmailAsync(resetEmail);
				}
				target = applicationUser;
				if (target == null)
				{
					setupLog.LogWarning("Setup__ResetPassword: no account {Email} to reset.", new object[] { string.IsNullOrWhiteSpace(resetEmail) ? "(owner)" : resetEmail });
				}
				else
				{
					IdentityResult identityResult = await userManager.ResetPasswordAsync(target, await userManager.GeneratePasswordResetTokenAsync(target), resetPassword);
					if (identityResult.Succeeded)
					{
						await userManager.SetLockoutEndDateAsync(target, null);
						await userManager.UpdateSecurityStampAsync(target);
						setupLog.LogWarning("Password for {Email} was reset from Setup__ResetPassword__Password. Remove the Setup__ResetPassword__ lines from absolut.env and restart, or this repeats on every start.", new object[] { target.Email });
					}
					else
					{
						ILogger logger = setupLog;
						string text3 = "Setup__ResetPassword for {Email} failed: {Errors}";
						object[] array = new object[2];
						array[0] = target.Email;
						array[1] = string.Join("; ", identityResult.Errors.Select<IdentityError, string>((IdentityError e) => e.Description));
						logger.LogWarning(text3, array);
					}
				}
			}
			resetEmail = null;
			setupLog = null;
			scope = null;
			userManager = null;
			target = null;
		}
		app.UseForwardedHeaders();
		if (app.Environment.IsDevelopment())
		{
			app.UseMigrationsEndPoint();
		}
		else
		{
			app.UseExceptionHandler("/Error", true);
			app.UseHsts();
		}
		app.UseHttpsRedirection();
		app.UseAntiforgery();
		app.MapStaticAssets(null);
		app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
		app.MapAdditionalIdentityEndpoints();
		app.Run(null);
	}

	// Token: 0x060000BB RID: 187 RVA: 0x000051C0 File Offset: 0x000033C0
	private static void <Main>(string[] args)
	{
		Program.<Main>$(args).GetAwaiter().GetResult();
	}
}
