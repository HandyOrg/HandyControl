using System.Windows;
using System.Windows.Media;

namespace HandyControl.Data;

public class MessageBoxInfo
{
    public string Message { get; set; }

    public string Caption { get; set; }

    public MessageBoxButton Button { get; set; } = MessageBoxButton.OK;

    public Geometry Icon { get; set; }

    public string IconKey { get; set; }

    public Brush IconBrush { get; set; }

    public string IconBrushKey { get; set; }

    public MessageBoxResult DefaultResult { get; set; } = MessageBoxResult.None;

    public Style Style { get; set; }

    public string StyleKey { get; set; }
}
