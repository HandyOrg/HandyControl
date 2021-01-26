using System.Windows;
using HandyControl.Controls;

namespace HandyControlDemo.UserControl
{
    public partial class PinBoxDemoCtl
    {
        public PinBoxDemoCtl()
        {
            InitializeComponent();
        }

        private void PinBox_OnCompleted(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is PinBox pinBox)
            {
                Growl.Info(pinBox.Password);
            }
        }
    }
}
