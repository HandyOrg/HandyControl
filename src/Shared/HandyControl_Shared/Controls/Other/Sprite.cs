using System.Windows;

namespace HandyControl.Controls
{
    public sealed class Sprite : System.Windows.Window
    {
        private Sprite()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
        }

        public static Sprite Show(object content)
        {
            var sprite = new Sprite
            {
                Content = content
            };

            sprite.Show();
            return sprite;
        }
    }
}
