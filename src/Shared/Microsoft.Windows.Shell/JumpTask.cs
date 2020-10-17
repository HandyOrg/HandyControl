using System;

namespace Microsoft.Windows.Shell
{
	// Token: 0x0200000A RID: 10
	public class JumpTask : JumpItem
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003439 File Offset: 0x00001639
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00003441 File Offset: 0x00001641
		public string Title { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000344A File Offset: 0x0000164A
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00003452 File Offset: 0x00001652
		public string Description { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000345B File Offset: 0x0000165B
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00003463 File Offset: 0x00001663
		public string ApplicationPath { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000047 RID: 71 RVA: 0x0000346C File Offset: 0x0000166C
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00003474 File Offset: 0x00001674
		public string Arguments { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000347D File Offset: 0x0000167D
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00003485 File Offset: 0x00001685
		public string WorkingDirectory { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000348E File Offset: 0x0000168E
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00003496 File Offset: 0x00001696
		public string IconResourcePath { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000349F File Offset: 0x0000169F
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000034A7 File Offset: 0x000016A7
		public int IconResourceIndex { get; set; }
	}
}
