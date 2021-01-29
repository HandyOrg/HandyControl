using System;
using System.ComponentModel;
using System.Windows;

namespace HandyControl.ThemeManager
{
    /// <summary>
    /// Enables you to detect whether your app is in design mode in a visual designer.
    /// </summary>
    internal static class DesignMode
    {
        private static readonly Lazy<bool> _designModeEnabled =
            new Lazy<bool>(() => DesignerProperties.GetIsInDesignMode(new DependencyObject()));

        /// <summary>
        /// Gets a value that indicates whether the process is running in design mode.
        /// </summary>
        /// <returns>**True** if the process is running in design mode; otherwise **false**.</returns>
        public static bool DesignModeEnabled => _designModeEnabled.Value;
    }
}
