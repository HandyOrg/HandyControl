using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HandyControl.Controls
{
    public class HamburgerTabControl : TabControl
    {
        public static readonly DependencyProperty TabPanelVerticalAlignmentProperty = ElementBase.Property<HamburgerTabControl, VerticalAlignment>(nameof(TabPanelVerticalAlignmentProperty), VerticalAlignment.Top);
        public static readonly DependencyProperty OffsetProperty = ElementBase.Property<HamburgerTabControl, Thickness>(nameof(OffsetProperty), new Thickness(0));
        public static readonly DependencyProperty IconModeProperty = ElementBase.Property<HamburgerTabControl, bool>(nameof(IconModeProperty), false);

        public static RoutedUICommand IconModeClickCommand = ElementBase.Command<HamburgerTabControl>(nameof(IconModeClickCommand));

        public VerticalAlignment TabPanelVerticalAlignment { get { return (VerticalAlignment)GetValue(TabPanelVerticalAlignmentProperty); } set { SetValue(TabPanelVerticalAlignmentProperty, value); } }
        public Thickness Offset { get { return (Thickness)GetValue(OffsetProperty); } set { SetValue(OffsetProperty, value); } }
        public bool IconMode { get { return (bool)GetValue(IconModeProperty); } set { SetValue(IconModeProperty, value); GoToState(); } }

        void GoToState()
        {
            ElementBase.GoToState(this, IconMode ? "EnterIconMode" : "ExitIconMode");
        }

        void SelectionState()
        {
            if (IconMode)
            {
                ElementBase.GoToState(this, "SelectionStartIconMode");
                ElementBase.GoToState(this, "SelectionEndIconMode");
            }
            else
            {
                ElementBase.GoToState(this, "SelectionStart");
                ElementBase.GoToState(this, "SelectionEnd");
            }
        }

        public HamburgerTabControl()
        {
            Loaded += delegate { GoToState(); ElementBase.GoToState(this, IconMode ? "SelectionLoadedIconMode" : "SelectionLoaded"); };
            SelectionChanged += delegate (object sender, SelectionChangedEventArgs e) { if (e.Source is HamburgerTabControl) { SelectionState(); } };
            CommandBindings.Add(new CommandBinding(IconModeClickCommand, delegate { IconMode = !IconMode; GoToState(); }));

            Utility.Refresh(this);
        }

        static HamburgerTabControl()
        {
            ElementBase.DefaultStyle<HamburgerTabControl>(DefaultStyleKeyProperty);
        }
    }
}
