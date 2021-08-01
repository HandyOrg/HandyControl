using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class RibbonGroup : HeaderedItemsControl
    {
        public static readonly DependencyProperty ShowLauncherButtonProperty = DependencyProperty.Register(
            "ShowLauncherButton", typeof(bool), typeof(RibbonGroup), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowLauncherButton
        {
            get => (bool) GetValue(ShowLauncherButtonProperty);
            set => SetValue(ShowLauncherButtonProperty, value);
        }

        public static readonly DependencyProperty ShowSplitterProperty = DependencyProperty.Register(
            "ShowSplitter", typeof(bool), typeof(RibbonGroup), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowSplitter
        {
            get => (bool) GetValue(ShowSplitterProperty);
            set => SetValue(ShowSplitterProperty, value);
        }
    }
}
