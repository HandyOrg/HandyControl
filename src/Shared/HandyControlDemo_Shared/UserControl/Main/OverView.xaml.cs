using System.Windows;
using HandyControl.Tools.Extension;

namespace HandyControlDemo.UserControl
{
    public partial class OverView
    {
        public OverView()
        {
            InitializeComponent();

            foreach (UIElement child in CanvasMain.Children)
            {
                child.Show();
            }
        }
    }
}
