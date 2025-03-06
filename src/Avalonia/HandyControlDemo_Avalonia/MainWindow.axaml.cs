using Avalonia.Controls;
using Avalonia.Interactivity;
using HandyControlDemo.UserControl;
using HandyControlDemo.ViewModel;

namespace HandyControlDemo;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        DataContext = ViewModelLocator.Instance.Main;
        ControlMain.Content = new MainWindowContent();
    }
}
