using System;

namespace Microsoft.Windows.Shell;

public enum JumpItemRejectionReason
{
    None,
    InvalidItem,
    NoRegisteredHandler,
    RemovedByUser
}
