using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Components.Pages
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class DeviceFormModel
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x00022DC2 File Offset: 0x00020FC2
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x00022DCA File Offset: 0x00020FCA
		[Nullable(1)]
		[Required]
		[StringLength(100)]
		public string Name
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "";

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x00022DD3 File Offset: 0x00020FD3
		// (set) Token: 0x06000687 RID: 1671 RVA: 0x00022DDB File Offset: 0x00020FDB
		public string IpAddress { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x00022DE4 File Offset: 0x00020FE4
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x00022DEC File Offset: 0x00020FEC
		public int Port { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x00022DF5 File Offset: 0x00020FF5
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x00022DFD File Offset: 0x00020FFD
		[Nullable(1)]
		public string SnmpVersion
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			set;
		} = "v2c";

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x00022E06 File Offset: 0x00021006
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x00022E0E File Offset: 0x0002100E
		public string SnmpCommunity { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00022E17 File Offset: 0x00021017
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x00022E1F File Offset: 0x0002101F
		public string SshUsername { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00022E28 File Offset: 0x00021028
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x00022E30 File Offset: 0x00021030
		public string SshPassword { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00022E39 File Offset: 0x00021039
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x00022E41 File Offset: 0x00021041
		public int? ParentDeviceId { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00022E4A File Offset: 0x0002104A
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x00022E52 File Offset: 0x00021052
		public string SerialNumber { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00022E5B File Offset: 0x0002105B
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x00022E63 File Offset: 0x00021063
		public string ChildIdentifier { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x00022E6C File Offset: 0x0002106C
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x00022E74 File Offset: 0x00021074
		public double? CoordinateX { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x00022E7D File Offset: 0x0002107D
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x00022E85 File Offset: 0x00021085
		public double? CoordinateY { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00022E8E File Offset: 0x0002108E
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x00022E96 File Offset: 0x00021096
		public double? CoordinateZ { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x00022E9F File Offset: 0x0002109F
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x00022EA7 File Offset: 0x000210A7
		public string SerialPort { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00022EB0 File Offset: 0x000210B0
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x00022EB8 File Offset: 0x000210B8
		[Range(300, 921600)]
		public int SerialBaudRate { get; set; } = 115200;

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00022EC1 File Offset: 0x000210C1
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00022EC9 File Offset: 0x000210C9
		public string SerialFraming { get; set; } = "8-N-1";

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00022ED2 File Offset: 0x000210D2
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x00022EDA File Offset: 0x000210DA
		[Range(5, 86400)]
		public int PollIntervalSeconds { get; set; } = 30;

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00022EE3 File Offset: 0x000210E3
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x00022EEB File Offset: 0x000210EB
		public bool Enabled { get; set; } = true;

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00022EF4 File Offset: 0x000210F4
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x00022EFC File Offset: 0x000210FC
		public string Notes { get; set; }
	}
}
