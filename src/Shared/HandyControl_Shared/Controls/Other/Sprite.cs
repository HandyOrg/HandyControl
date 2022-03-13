using System.Windows;

namespace HandyControl.Controls;

public sealed class Sprite : System.Windows.Window
{
    private Sprite()
    {
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
    }

    public static Sprite Show(object content)
    {
        var sprite = new Sprite
        {
            Content = content
        };

        sprite.Show();

        var desktopWorkingArea = SystemParameters.WorkArea;
        sprite.Left = desktopWorkingArea.Width - sprite.ActualWidth - 50;
        sprite.Top = 50 - sprite.Padding.Top;

        return sprite;
    }
}
