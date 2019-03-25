using System.Security;
using System.Security.Permissions;

namespace HandyControl.Tools
{
    internal class SecurityHelper
    {
        private static UIPermission _allWindowsUIPermission;

        [SecurityCritical]
        internal static void DemandUIWindowPermission()
        {
            if (_allWindowsUIPermission == null)
            {
                _allWindowsUIPermission = new UIPermission(UIPermissionWindow.AllWindows);
            }
            _allWindowsUIPermission.Demand();
        }
    }
}