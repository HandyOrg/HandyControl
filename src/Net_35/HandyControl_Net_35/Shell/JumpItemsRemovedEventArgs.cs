namespace Microsoft.Windows.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public sealed class JumpItemsRemovedEventArgs : EventArgs
    {
        public JumpItemsRemovedEventArgs() : this(null)
        {
        }

        public JumpItemsRemovedEventArgs(IList<JumpItem> removedItems)
        {
            if (removedItems != null)
            {
                this.RemovedItems = new List<JumpItem>(removedItems).AsReadOnly();
            }
            else
            {
                this.RemovedItems = new List<JumpItem>().AsReadOnly();
            }
        }

        public IList<JumpItem> RemovedItems { get; private set; }
    }
}

