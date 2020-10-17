using System;
using System.Collections.Generic;

namespace Microsoft.Windows.Shell
{
	// Token: 0x02000004 RID: 4
	public sealed class JumpItemsRejectedEventArgs : EventArgs
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		public JumpItemsRejectedEventArgs() : this(null, null)
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
		public JumpItemsRejectedEventArgs(IList<JumpItem> rejectedItems, IList<JumpItemRejectionReason> reasons)
		{
			if ((rejectedItems == null && reasons != null) || (reasons == null && rejectedItems != null) || (rejectedItems != null && reasons != null && rejectedItems.Count != reasons.Count))
			{
				throw new ArgumentException("The counts of rejected items doesn't match the count of reasons.");
			}
			if (rejectedItems != null)
			{
				this.RejectedItems = new List<JumpItem>(rejectedItems).AsReadOnly();
				this.RejectionReasons = new List<JumpItemRejectionReason>(reasons).AsReadOnly();
				return;
			}
			this.RejectedItems = new List<JumpItem>().AsReadOnly();
			this.RejectionReasons = new List<JumpItemRejectionReason>().AsReadOnly();
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020F8 File Offset: 0x000002F8
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002100 File Offset: 0x00000300
		public IList<JumpItem> RejectedItems { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002109 File Offset: 0x00000309
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002111 File Offset: 0x00000311
		public IList<JumpItemRejectionReason> RejectionReasons { get; private set; }
	}
}
