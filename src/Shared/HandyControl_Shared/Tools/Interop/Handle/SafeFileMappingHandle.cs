using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace HandyControl.Tools.Interop
{
    internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [SecurityCritical]
        internal SafeFileMappingHandle(IntPtr handle) : base(false)
        {
            SetHandle(handle);
        }

        [SecurityCritical, SecuritySafeCritical]
        internal SafeFileMappingHandle() : base(true)
        {
        }

        public override bool IsInvalid
        {
            [SecurityCritical, SecuritySafeCritical]
            get => handle == IntPtr.Zero;
        }

        [SecurityCritical, SecuritySafeCritical]
        protected override bool ReleaseHandle()
        {
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
            try
            {
                return CloseHandleNoThrow(new HandleRef(null, handle));
            }
            finally
            {
                CodeAccessPermission.RevertAssert();
            }
        }

        [SecurityCritical]
        public static bool CloseHandleNoThrow(HandleRef handle)
        {
            HandleCollector.Remove((IntPtr)handle, CommonHandles.Kernel);
            var result = InteropMethods.IntCloseHandle(handle);
            return result;
        }
    }
}