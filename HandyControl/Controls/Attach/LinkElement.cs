using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    public class LinkElement
    {
        public static readonly DependencyProperty LinkProperty = DependencyProperty.RegisterAttached(
            "Link", typeof(string), typeof(LinkElement), new PropertyMetadata(default(string), OnLinkChanged));

        private static void OnLinkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                CreateTrigger(element, (string) e.NewValue);
            }
        }

        private static readonly DependencyProperty CommandBindingProperty = DependencyProperty.RegisterAttached(
            "CommandBinding", typeof(CommandBinding), typeof(LinkElement), new PropertyMetadata(default(CommandBinding)));

        private static void SetCommandBinding(DependencyObject element, CommandBinding value)
            => element.SetValue(CommandBindingProperty, value);

        private static CommandBinding GetCommandBinding(DependencyObject element)
            => (CommandBinding) element.GetValue(CommandBindingProperty);

        private static void CreateTrigger(UIElement element, string link)
        {
            element.CommandBindings.Remove(GetCommandBinding(element));
            element.SetCurrentValue(CommandBindingProperty, DependencyProperty.UnsetValue);

            if (string.IsNullOrEmpty(link)) return;
            var commandBinding = new CommandBinding(ControlCommands.OpenLink, (sender, args) => Process.Start(link));
            SetCommandBinding(element, commandBinding);
            element.CommandBindings.Add(commandBinding);
        }

        public static void SetLink(DependencyObject element, string value)
            => element.SetValue(LinkProperty, value);

        public static string GetLink(DependencyObject element)
            => (string) element.GetValue(LinkProperty);
    }
}