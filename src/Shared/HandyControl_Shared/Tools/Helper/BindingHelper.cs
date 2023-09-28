using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HandyControl.Tools.Helper;

public class BindingHelper
{
    public static string GetString(object source, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return source == null ? string.Empty : source.ToString();
        }

        var tempObj = new DependencyObject();

        var binding = new Binding(path)
        {
            Mode = BindingMode.OneTime,
            Source = source
        };

        BindingOperations.SetBinding(tempObj, TextSearch.TextProperty, binding);
        var result = (string) tempObj.GetValue(TextSearch.TextProperty);

        BindingOperations.ClearBinding(tempObj, TextSearch.TextProperty);
        return result;
    }
}
