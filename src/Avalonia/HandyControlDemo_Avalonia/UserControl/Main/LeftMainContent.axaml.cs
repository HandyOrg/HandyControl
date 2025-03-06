using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class LeftMainContent : Avalonia.Controls.UserControl
{
    public LeftMainContent()
    {
        InitializeComponent();
    }

    private void TabControl_OnSelectionChanged(object? _, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0)
        {
            return;
        }

        if (e.AddedItems[0] is DemoItemModel demoItem)
        {
            WeakReferenceMessenger.Default.Send(demoItem, MessageToken.SwitchDemo);
        }
    }
}
