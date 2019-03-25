using System;
using System.Security;
using System.Security.Permissions;

namespace HandyControl.Tools.Interop
{
    internal class IntSecurity
    {
        public static CodeAccessPermission UnrestrictedWindows =>
            new Lazy<CodeAccessPermission>(() => AllWindows).Value;

        public static CodeAccessPermission AllWindows =>
            new Lazy<CodeAccessPermission>(() => new UIPermission(UIPermissionWindow.AllWindows)).Value;

        public static CodeAccessPermission CreateAnyWindow =>
            new Lazy<CodeAccessPermission>(() => SafeSubWindows).Value;

        public static CodeAccessPermission SafeSubWindows =>
            new Lazy<CodeAccessPermission>(() => new UIPermission(UIPermissionWindow.SafeSubWindows)).Value;
    }
}