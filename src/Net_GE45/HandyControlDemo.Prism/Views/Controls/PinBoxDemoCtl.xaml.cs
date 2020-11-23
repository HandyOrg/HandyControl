using HandyControl.Controls;
using System.Windows;

namespace HandyControlDemo.Views
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
