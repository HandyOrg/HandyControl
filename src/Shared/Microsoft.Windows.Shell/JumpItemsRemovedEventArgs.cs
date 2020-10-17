using System;
using System.Collections.Generic;

namespace Microsoft.Windows.Shell
{
	// Token: 0x02000005 RID: 5
	public sealed class JumpItemsRemovedEventArgs : EventArgs
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000211A File Offset: 0x0000031A
		public JumpItemsRemovedEventArgs() : this(null)
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002123 File Offset: 0x00000323
		public JumpItemsRemovedEventArgs(IList<JumpItem> removedItems)
		{
			if (removedItems != null)
			{
				this.RemovedItems = new List<JumpItem>(removedItems).AsReadOnly();
				return;
			}
			this.RemovedItems = new List<JumpItem>().AsReadOnly();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002150 File Offset: 0x00000350
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002158 File Offset: 0x00000358
		public IList<JumpItem> RemovedItems { get; private set; }
	}
}
