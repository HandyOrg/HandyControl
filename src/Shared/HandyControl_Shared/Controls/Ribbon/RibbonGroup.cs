using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    public class RibbonGroup : HeaderedItemsControl
    {
        public RibbonGroup()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.More, LauncherButton_OnClick));
        }

        private void LauncherButton_OnClick(object sender, ExecutedRoutedEventArgs e)
        {
            OnLauncherClick(new RoutedEventArgs(LauncherClickEvent, this));
        }

        public static readonly DependencyProperty ShowLauncherButtonProperty = DependencyProperty.Register(
            nameof(ShowLauncherButton), typeof(bool), typeof(RibbonGroup), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowLauncherButton
        {
            get => (bool) GetValue(ShowLauncherButtonProperty);
            set => SetValue(ShowLauncherButtonProperty, value);
        }

        public static readonly DependencyProperty ShowSplitterProperty = DependencyProperty.Register(
            nameof(ShowSplitter), typeof(bool), typeof(RibbonGroup), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowSplitter
        {
            get => (bool) GetValue(ShowSplitterProperty);
            set => SetValue(ShowSplitterProperty, value);
        }

        public static readonly DependencyProperty LauncherPoptipProperty = DependencyProperty.Register(
            nameof(LauncherPoptip), typeof(Poptip), typeof(RibbonGroup), new PropertyMetadata(default(Poptip)));

        public Poptip LauncherPoptip
        {
            get => (Poptip) GetValue(LauncherPoptipProperty);
            set => SetValue(LauncherPoptipProperty, value);
        }

        public static readonly RoutedEvent LauncherClickEvent =
            EventManager.RegisterRoutedEvent("LauncherClick", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(RibbonGroup));

        public event RoutedEventHandler LauncherClick
        {
            add => AddHandler(LauncherClickEvent, value);
            remove => RemoveHandler(LauncherClickEvent, value);
        }

        protected virtual void OnLauncherClick(RoutedEventArgs e) => RaiseEvent(e);
    }
}
