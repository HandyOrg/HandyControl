using Avalonia.Controls;
using Avalonia.Interactivity;
using HandyControl.Controls;
using HandyControlDemo.Data;
using HandyControlDemo.UserControl;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo;

public partial class MainWindow : Avalonia.Controls.Window
{
    public MainWindow()
    {
        InitializeComponent();

        Dialog.SetToken(this, MessageToken.MainWindow);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        DataContext = ViewModelLocator.Instance.Main;
        ControlMain.Content = new MainWindowContent();
    }
}
