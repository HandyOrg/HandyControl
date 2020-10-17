using System;

namespace Microsoft.Windows.Shell
{
	// Token: 0x02000002 RID: 2
	public abstract class JumpItem
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal JumpItem()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		// (set) Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public string CustomCategory { get; set; }
	}
}
