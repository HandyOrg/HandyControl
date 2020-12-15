using System.Windows;
using System.Windows.Input;
using HandyControl.Tools.Helper;

namespace HandyControlDemo.Window
{
    public partial class TouchDragMoveWindow
    {
        public TouchDragMoveWindow()
        {
            InitializeComponent();

            TouchDown += MainWindow_TouchDown;
            TouchUp += MainWindow_TouchUp;
        }

        private void MainWindow_TouchUp(object sender, TouchEventArgs e)
        {
            _currentTouchCount--;
        }

        private void MainWindow_TouchDown(object sender, TouchEventArgs e)
        {
            CaptureTouch(e.TouchDevice);

            if (_currentTouchCount == 0)
            {
                TouchDragMoveWindowHelper.DragMove(this);
            }

            _currentTouchCount++;
        }

        private uint _currentTouchCount;
    }
}
