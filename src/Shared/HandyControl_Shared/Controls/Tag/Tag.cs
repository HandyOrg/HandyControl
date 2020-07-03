using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls
{
    public class Tag : ContentControl
    {
        public Tag()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Close, (s, e) =>
            {
                var argsClosing = new CancelRoutedEventArgs(ClosingEvent, this);
                RaiseEvent(argsClosing);
                if (argsClosing.Cancel) return;

                RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
            }));
        }

        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            "ShowCloseButton", typeof(bool), typeof(Tag), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowCloseButton
        {
            get => (bool) GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty SelectableProperty = DependencyProperty.Register(
            "Selectable", typeof(bool), typeof(Tag), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool Selectable
        {
            get => (bool) GetValue(SelectableProperty);
            set => SetValue(SelectableProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(Tag), new PropertyMetadata(ValueBoxes.FalseBox, (o, args) =>
            {
                var ctl = (Tag) o;
                ctl.RaiseEvent(new RoutedEventArgs(SelectedEvent, ctl));
            }));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(EventHandler), typeof(Tag));

        public event EventHandler Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }

        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(EventHandler), typeof(Tag));

        public event EventHandler Closing
        {
            add => AddHandler(ClosingEvent, value);
            remove => RemoveHandler(ClosingEvent, value);
        }

        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(EventHandler), typeof(Tag));

        public event EventHandler Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (Selectable)
            {
                IsSelected = true;
            }
        }
    }
}