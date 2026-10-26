using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Data.Entities
{
	// Token: 0x0200008B RID: 139
	[NullableContext(2)]
	[Nullable(0)]
	public class Device
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00018D57 File Offset: 0x00016F57
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x00018D5F File Offset: 0x00016F5F
		public int Id { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00018D68 File Offset: 0x00016F68
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x00018D70 File Offset: 0x00016F70
		[Nullable(1)]
		public string Name
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00018D79 File Offset: 0x00016F79
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x00018D81 File Offset: 0x00016F81
		public int CategoryId { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00018D8A File Offset: 0x00016F8A
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00018D92 File Offset: 0x00016F92
		[Nullable(1)]
		public DeviceCategory Category
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00018D9B File Offset: 0x00016F9B
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x00018DA3 File Offset: 0x00016FA3
		public int TemplateId { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00018DAC File Offset: 0x00016FAC
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00018DB4 File Offset: 0x00016FB4
		[Nullable(1)]
		public DeviceTemplate Template
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00018DBD File Offset: 0x00016FBD
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00018DC5 File Offset: 0x00016FC5
		public CollectionMethod CollectionMethod { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00018DCE File Offset: 0x00016FCE
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x00018DD6 File Offset: 0x00016FD6
		public string IpAddress { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00018DDF File Offset: 0x00016FDF
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x00018DE7 File Offset: 0x00016FE7
		public int Port { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00018DF0 File Offset: 0x00016FF0
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00018DF8 File Offset: 0x00016FF8
		[Nullable(1)]
		public string SnmpVersion
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "v2c";

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00018E01 File Offset: 0x00017001
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x00018E09 File Offset: 0x00017009
		public string SnmpCommunityEnc { get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00018E12 File Offset: 0x00017012
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x00018E1A File Offset: 0x0001701A
		public string SshUsername { get; set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00018E23 File Offset: 0x00017023
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x00018E2B File Offset: 0x0001702B
		public string SshPasswordEnc { get; set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00018E34 File Offset: 0x00017034
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x00018E3C File Offset: 0x0001703C
		public int? ParentDeviceId { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00018E45 File Offset: 0x00017045
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00018E4D File Offset: 0x0001704D
		public Device ParentDevice { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00018E56 File Offset: 0x00017056
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00018E5E File Offset: 0x0001705E
		[Nullable(1)]
		public ICollection<Device> Children
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = new List<Device>();

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00018E67 File Offset: 0x00017067
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00018E6F File Offset: 0x0001706F
		public string SerialNumber { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00018E78 File Offset: 0x00017078
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x00018E80 File Offset: 0x00017080
		public string ChildIdentifier { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00018E89 File Offset: 0x00017089
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x00018E91 File Offset: 0x00017091
		public double? CoordinateX { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00018E9A File Offset: 0x0001709A
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x00018EA2 File Offset: 0x000170A2
		public double? CoordinateY { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00018EAB File Offset: 0x000170AB
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00018EB3 File Offset: 0x000170B3
		public double? CoordinateZ { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00018EBC File Offset: 0x000170BC
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00018EC4 File Offset: 0x000170C4
		public DateTime? ConfigChangedAt { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00018ECD File Offset: 0x000170CD
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00018ED5 File Offset: 0x000170D5
		public DateTime? ConfigRestartedAt { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00018EE0 File Offset: 0x000170E0
		public bool ConfigRestartPending
		{
			get
			{
				DateTime? dateTime = this.ConfigChangedAt;
				if (dateTime == null)
				{
					return false;
				}
				DateTime valueOrDefault = dateTime.GetValueOrDefault();
				dateTime = this.ConfigRestartedAt;
				if (dateTime != null)
				{
					DateTime valueOrDefault2 = dateTime.GetValueOrDefault();
					return valueOrDefault > valueOrDefault2;
				}
				return true;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00018F28 File Offset: 0x00017128
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x00018F30 File Offset: 0x00017130
		public string SerialPort { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00018F39 File Offset: 0x00017139
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x00018F41 File Offset: 0x00017141
		public int SerialBaudRate { get; set; } = 115200;

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00018F4A File Offset: 0x0001714A
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00018F52 File Offset: 0x00017152
		public string SerialFraming { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00018F5B File Offset: 0x0001715B
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00018F63 File Offset: 0x00017163
		public bool Enabled { get; set; } = true;

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00018F6C File Offset: 0x0001716C
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x00018F74 File Offset: 0x00017174
		public int PollIntervalSeconds { get; set; } = 30;

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00018F7D File Offset: 0x0001717D
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00018F85 File Offset: 0x00017185
		public string Notes { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00018F8E File Offset: 0x0001718E
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x00018F96 File Offset: 0x00017196
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00018F9F File Offset: 0x0001719F
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x00018FA7 File Offset: 0x000171A7
		public DateTime? LastPolledAt { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00018FB0 File Offset: 0x000171B0
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00018FB8 File Offset: 0x000171B8
		public bool? LastReachable { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00018FC1 File Offset: 0x000171C1
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x00018FC9 File Offset: 0x000171C9
		public string LastError { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00018FD2 File Offset: 0x000171D2
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x00018FDA File Offset: 0x000171DA
		public string LastSnapshotJson { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00018FE3 File Offset: 0x000171E3
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x00018FEB File Offset: 0x000171EB
		[Nullable(1)]
		public ICollection<DevicePollResult> PollResults
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = new List<DevicePollResult>();
	}
}
