using Avalonia.Interactivity;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControlDemo.UserControl;

public partial class SearchBarDemo : Avalonia.Controls.UserControl
{
    public SearchBarDemo()
    {
        InitializeComponent();
    }

    private void SearchBar_OnSearchStarted(object? sender, FunctionEventArgs<string?> e)
    {
        if (string.IsNullOrWhiteSpace(e.Info))
        {
            return;
        }

        var growlType = System.Type.GetType("HandyControl.Controls.Growl, HandyControl");
        var method = growlType?.GetMethod("Info", [typeof(string), typeof(string)]);
        method?.Invoke(null, [e.Info, string.Empty]);
    }
}
