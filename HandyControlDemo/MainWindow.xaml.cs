using HandyControl.Controls;

namespace HandyControlDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Growl.SetGrowlPanel(PanelMessage);
        }
    }
}