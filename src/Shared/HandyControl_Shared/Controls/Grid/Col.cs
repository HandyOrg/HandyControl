using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    public class Col : ContentControl
    {
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
            "Layout", typeof(ColLayout), typeof(Col), new PropertyMetadata(new ColLayout()));

        public ColLayout Layout
        {
            get => (ColLayout)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            "Offset", typeof(int), typeof(Col), new PropertyMetadata(default(int)));

        public int Offset
        {
            get => (int)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        internal int GetLayoutCellCount(ColLayoutStatus status)
        {
            switch (status)
            {
                case ColLayoutStatus.Xs: return Layout.Xs;
                case ColLayoutStatus.Sm: return Layout.Sm;
                case ColLayoutStatus.Md: return Layout.Md;
                case ColLayoutStatus.Lg: return Layout.Lg;
                case ColLayoutStatus.Xl: return Layout.Xl;
                case ColLayoutStatus.Xxl: return Layout.Xxl;
                default: return 0;
            }
        }
    }
}
