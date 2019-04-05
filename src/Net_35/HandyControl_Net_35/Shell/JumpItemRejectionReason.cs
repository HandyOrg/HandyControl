namespace Microsoft.Windows.Shell
{
    using System;

    public enum JumpItemRejectionReason
    {
        None,
        InvalidItem,
        NoRegisteredHandler,
        RemovedByUser
    }
}

