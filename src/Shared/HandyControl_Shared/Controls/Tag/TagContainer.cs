using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class TagContainer : ItemsControl
    {
        public static readonly DependencyProperty IsToolBarVisibleProperty = DependencyProperty.Register(
            "IsToolBarVisible", typeof(bool), typeof(TagContainer), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsToolBarVisible
        {
            get => (bool) GetValue(IsToolBarVisibleProperty);
            set => SetValue(IsToolBarVisibleProperty, value);
        }

        protected override DependencyObject GetContainerForItemOverride() => new Tag();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is Tag;
    }
}