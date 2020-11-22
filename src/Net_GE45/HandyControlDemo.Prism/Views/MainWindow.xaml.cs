using HandyControlDemo.Data;
using System.ComponentModel;
using MessageBox = HandyControl.Controls.MessageBox;

namespace HandyControlDemo.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (GlobalData.NotifyIconIsShow)
            {
                MessageBox.Info(Properties.Langs.Lang.AppClosingTip, Properties.Langs.Lang.Tip);
                Hide();
                e.Cancel = true;
            }
            else
            {
                base.OnClosing(e);
            }
        }
    }
}
