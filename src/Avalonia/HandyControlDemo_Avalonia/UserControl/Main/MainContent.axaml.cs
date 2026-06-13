using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class MainContent : Avalonia.Controls.UserControl
{
    private bool _isFull;

    public MainContent()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<ValueChangedMessage<bool>, string>(this, MessageToken.FullSwitch, (_, m) => FullSwitch(m.Value));
    }

    private void FullSwitch(bool isFull)
    {
        if (_isFull == isFull)
        {
            return;
        }

        _isFull = isFull;

        if (_isFull)
        {
            BorderEffect.IsVisible = false;
            BorderTitle.IsVisible = false;
            GridMain.HorizontalAlignment = HorizontalAlignment.Stretch;
            GridMain.VerticalAlignment = VerticalAlignment.Stretch;
            PresenterMain.Margin = new Thickness();
            BorderRoot.Margin = new Thickness();
        }
        else
        {
            BorderEffect.IsVisible = true;
            BorderTitle.IsVisible = true;
            GridMain.HorizontalAlignment = HorizontalAlignment.Center;
            GridMain.VerticalAlignment = VerticalAlignment.Center;
            PresenterMain.Margin = new Thickness(0, 0, 0, 10);
            BorderRoot.Margin = new Thickness(16);
        }
    }
}
