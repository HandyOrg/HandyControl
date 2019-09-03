using System.Windows;
using System.Windows.Controls.Primitives;
using HandyControl.Data.Enum;

namespace HandyControl.Controls
{
    public class SplitButton : ButtonBase
    {
        public static readonly DependencyProperty HitModeProperty = DependencyProperty.Register(
            "HitMode", typeof(MouseHitMode), typeof(SplitButton), new PropertyMetadata(default(MouseHitMode)));

        public MouseHitMode HitMode
        {
            get => (MouseHitMode) GetValue(HitModeProperty);
            set => SetValue(HitModeProperty, value);
        }
    }
}
