using Avalonia.Controls;
using Avalonia.Interactivity;
using HandyControl.Controls;

namespace HandyControlDemo.UserControl;

public partial class BadgeDemo : Avalonia.Controls.UserControl
{
    public BadgeDemo()
    {
        InitializeComponent();

        var badge = this.FindControl<Badge>("CountBadge");
        var button = this.FindControl<Button>("CountButton");
        if (button != null && badge != null)
        {
            button.Click += (_, _) => badge.Value += 1;
        }
    }
}
