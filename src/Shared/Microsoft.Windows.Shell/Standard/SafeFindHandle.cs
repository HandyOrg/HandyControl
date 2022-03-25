using System;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Standard;

internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    private SafeFindHandle() : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
        return NativeMethods.FindClose(this.handle);
    }
}
