using System.Windows;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using MessageBox = HandyControl.Controls.MessageBox;

namespace HandyControlDemo.UserControl;

public partial class WindowDemo
{
    public WindowDemo()
    {
        InitializeComponent();
    }

    private void ButtonMessage_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(Properties.Langs.Lang.GrowlAsk, Properties.Langs.Lang.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
    }

    private void ButtonCustomMessage_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(new MessageBoxInfo
        {
            Message = Properties.Langs.Lang.GrowlAsk,
            Caption = Properties.Langs.Lang.Title,
            Button = MessageBoxButton.YesNo,
            IconBrushKey = ResourceToken.AccentBrush,
            IconKey = ResourceToken.AskGeometry,
            StyleKey = "MessageBoxCustom"
        });
    }

    private void ButtonMouseFollow_OnClick(object sender, RoutedEventArgs e)
    {
        var picker = SingleOpenHelper.CreateControl<ColorPicker>();
        var window = new PopupWindow
        {
            PopupElement = picker
        };
        picker.SelectedColorChanged += delegate { window.Close(); };
        picker.Canceled += delegate { window.Close(); };
        window.Show(ButtonMouseFollow, false);
    }

    private void ButtonCustomContent_OnClick(object sender, RoutedEventArgs e)
    {
        var picker = SingleOpenHelper.CreateControl<ColorPicker>();
        var window = new PopupWindow
        {
            PopupElement = picker,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            AllowsTransparency = true,
            WindowStyle = WindowStyle.None,
            MinWidth = 0,
            MinHeight = 0,
            Title = Properties.Langs.Lang.ColorPicker
        };
        picker.SelectedColorChanged += delegate { window.Close(); };
        picker.Canceled += delegate { window.Close(); };
        window.Show();
    }
}
