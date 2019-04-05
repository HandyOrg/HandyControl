using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Tools.Extension;
using HandyControlDemo.Data;


namespace HandyControlDemo.UserControl
{
    /// <summary>
    ///     主内容
    /// </summary>
    public partial class MainContent
    {
        private bool _isFull;

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
    }
}