using System;
using System.Collections.Generic;

namespace Microsoft.Windows.Shell;

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
            return;
        }
        this.RemovedItems = new List<JumpItem>().AsReadOnly();
    }

    public IList<JumpItem> RemovedItems { get; private set; }
}
