using System.Windows;
using System.Windows.Input;

namespace HandyControl.Controls
{
    public sealed class Sprite : System.Windows.Window
    {
        private bool _isLeftButtonPressed;

        private double _leftMax;

        private double _topMax;

        private Sprite()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            InputManager.Current.PostProcessInput += Current_PostProcessInput;
        }

        private void Current_PostProcessInput(object sender, ProcessInputEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _isLeftButtonPressed = true;
            }
            else if (_isLeftButtonPressed)
            {
                if (Left < 0)
                {
                    Left = 0;
                }

                if (Left > _leftMax)
                {
                    Left = _leftMax;
                }

                if (Top > _topMax)
                {
                    Top = _topMax;
                }
            }
        }

        public static Sprite Show(object content)
        {
            var sprite = new Sprite
            {
                Content = content
            };

            sprite.Show();

            var desktopWorkingArea = SystemParameters.WorkArea;
            sprite._leftMax = desktopWorkingArea.Width - sprite.ActualWidth;
            sprite._topMax = desktopWorkingArea.Height - sprite.ActualHeight;
            sprite.Left = sprite._leftMax - 50;
            sprite.Top = 50 - sprite.Padding.Top;

            return sprite;
        }
    }
}
