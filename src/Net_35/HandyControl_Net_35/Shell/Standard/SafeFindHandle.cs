namespace Standard
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Security.Permissions;

    internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
        private SafeFindHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return Standard.NativeMethods.FindClose(base.handle);
        }
    }
}

