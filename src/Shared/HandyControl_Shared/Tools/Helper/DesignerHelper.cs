using System.ComponentModel;
using System.Windows;

namespace HandyControl.Tools;

public class DesignerHelper
{
    private static bool? _isInDesignMode;

    public static bool IsInDesignMode
    {
        get
        {
            if (!_isInDesignMode.HasValue)
            {
                _isInDesignMode = (bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
            return _isInDesignMode.Value;
        }
    }
}
