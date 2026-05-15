using System.Threading.Tasks;
using Avalonia.Interactivity;
using HandyControl.Controls;
using HandyControl.Data;

namespace HandyControlDemo.UserControl;

public partial class WindowDemo : Avalonia.Controls.UserControl
{
    public WindowDemo()
    {
        InitializeComponent();
    }


    private async void ButtonMessage_OnClick(object sender, RoutedEventArgs e)
    {
       await MessageBox.Show(Properties.Langs.Lang.GrowlAsk, Properties.Langs.Lang.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
    }
}

