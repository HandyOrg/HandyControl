using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data;
using HandyControlDemo.ViewModel;


namespace HandyControlDemo.UserControl
{
    /// <summary>
    ///     主内容
    /// </summary>
    public partial class MainContent
    {
        private bool _isFull;

        private string _currentDemoKey;

        public MainContent()
        {
            InitializeComponent();

            Messenger.Default.Register<bool>(this, MessageToken.FullSwitch, FullSwitch);
        }

        private void FullSwitch(bool isFull)
        {
            if (_isFull == isFull) return;
            _isFull = isFull;
            if (_isFull)
            {
                BorderEffect.Collapse();
                BorderTitle.Collapse();
                GridMain.HorizontalAlignment = HorizontalAlignment.Stretch;
                GridMain.VerticalAlignment = VerticalAlignment.Stretch;
                GridMain.Margin = new Thickness();
                PresenterMain.Margin = new Thickness();
            }
            else
            {
                BorderEffect.Show();
                BorderTitle.Show();
                GridMain.HorizontalAlignment = HorizontalAlignment.Center;
                GridMain.VerticalAlignment = VerticalAlignment.Center;
                GridMain.Margin = new Thickness(16);
                PresenterMain.Margin = new Thickness(0, 0, 0, 10);
            }
        }

        private void DrawerCode_OnOpened(object sender, RoutedEventArgs e)
        {
            var typeKey = ViewModelLocator.Instance.Main.DemoInfoCurrent.Key;
            var demoKey = ViewModelLocator.Instance.Main.DemoItemCurrent.TargetCtlName;
            if (Equals(_currentDemoKey, demoKey)) return;
            _currentDemoKey = demoKey;

            if (ViewModelLocator.Instance.Main.SubContent is FrameworkElement demoCtl)
            {
                var xamlPath = $"UserControl/{typeKey}/{demoCtl.GetType().Name}.xaml";
                var dc = demoCtl.DataContext;
                var dcTypeName = dc.GetType().Name;
                var vmPath = $"ViewModel/{dcTypeName}";

                EditorXaml.Text = DemoHelper.GetCode(xamlPath);
                EditorCs.Text = DemoHelper.GetCode($"{xamlPath}.cs");
                EditorVm.Text = DemoHelper.GetCode($"{vmPath}.cs");
            }
        }
    }
}