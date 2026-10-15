using System;
using System.Reflection;
using Avalonia.Interactivity;
using HandyControl.Controls;

namespace HandyControlDemo.UserControl;

public partial class PinBoxDemo : Avalonia.Controls.UserControl
{
    public PinBoxDemo()
    {
        InitializeComponent();
    }

    private void PinBox_OnCompleted(object? sender, RoutedEventArgs e)
    {
        if (e.Source is PinBox pinBox)
        {
            TryShowGrowl(pinBox.Password);
        }
    }

    private static void TryShowGrowl(string message)
    {
        var growlType = Type.GetType("HandyControl.Controls.Growl, HandyControl");
        var infoMethod = growlType?.GetMethod("Info", BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(string)]);
        infoMethod?.Invoke(null, [message, string.Empty]);
    }
}
