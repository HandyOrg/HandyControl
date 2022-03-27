using System.Windows.Controls;


namespace HandyControl.Controls;

/// <summary>
///     带上下文菜单的按钮
/// </summary>
public class ContextMenuButton : Button
{
    public ContextMenu Menu { get; set; }

    protected override void OnClick()
    {
        base.OnClick();
        if (Menu != null)
        {
            Menu.PlacementTarget = this;
            Menu.IsOpen = true;
        }
    }
}
