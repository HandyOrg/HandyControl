namespace Microsoft.Windows.Shell
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    [DefaultEvent("Click")]
    public sealed class ThumbButtonInfo : Freezable, ICommandSource
    {
        private static readonly DependencyProperty _CanExecuteProperty;
        private EventHandler _commandEvent;
        public static readonly DependencyProperty CommandParameterProperty;
        public static readonly DependencyProperty CommandProperty;
        public static readonly DependencyProperty CommandTargetProperty;
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ThumbButtonInfo), new PropertyMetadata(string.Empty, null, new CoerceValueCallback(ThumbButtonInfo._CoerceDescription)));
        public static readonly DependencyProperty DismissWhenClickedProperty = DependencyProperty.Register("DismissWhenClicked", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(false));
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(System.Windows.Media.ImageSource), typeof(ThumbButtonInfo), new PropertyMetadata(null));
        public static readonly DependencyProperty IsBackgroundVisibleProperty = DependencyProperty.Register("IsBackgroundVisible", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, null, (d, e) => ((ThumbButtonInfo) d)._CoerceIsEnabledValue(e)));
        public static readonly DependencyProperty IsInteractiveProperty = DependencyProperty.Register("IsInteractive", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(System.Windows.Visibility), typeof(ThumbButtonInfo), new PropertyMetadata(System.Windows.Visibility.Visible));

        public event EventHandler Click;

        static ThumbButtonInfo()
        {
            CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ThumbButtonInfo), new PropertyMetadata(null, (d, e) => ((ThumbButtonInfo) d)._OnCommandChanged(e)));
            CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ThumbButtonInfo), new PropertyMetadata(null, (d, e) => ((ThumbButtonInfo) d)._UpdateCanExecute()));
            CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ThumbButtonInfo), new PropertyMetadata(null, (d, e) => ((ThumbButtonInfo) d)._UpdateCanExecute()));
            _CanExecuteProperty = DependencyProperty.Register("_CanExecute", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, (d, e) => d.CoerceValue(IsEnabledProperty)));
        }

        private static object _CoerceDescription(DependencyObject d, object value)
        {
            string str = (string) value;
            if ((str != null) && (str.Length >= 260))
            {
                str = str.Substring(0, 0x103);
            }
            return str;
        }

        private object _CoerceIsEnabledValue(object value)
        {
            return (!((bool) value) ? ((object) 0) : ((object) this._CanExecute));
        }

        private void _HookCommand(ICommand command)
        {
            this._commandEvent = (sender, e) => this._UpdateCanExecute();
            command.CanExecuteChanged += this._commandEvent;
            this._UpdateCanExecute();
        }

        private void _InvokeCommand()
        {
            ICommand command = this.Command;
            if (command != null)
            {
                object commandParameter = this.CommandParameter;
                IInputElement commandTarget = this.CommandTarget;
                RoutedCommand command2 = command as RoutedCommand;
                if (command2 != null)
                {
                    if (command2.CanExecute(commandParameter, commandTarget))
                    {
                        command2.Execute(commandParameter, commandTarget);
                    }
                }
                else if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
            }
        }

        private void _OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            ICommand oldValue = (ICommand) e.OldValue;
            ICommand newValue = (ICommand) e.NewValue;
            if (oldValue != newValue)
            {
                if (oldValue != null)
                {
                    this._UnhookCommand(oldValue);
                }
                if (newValue != null)
                {
                    this._HookCommand(newValue);
                }
            }
        }

        private void _UnhookCommand(ICommand command)
        {
            command.CanExecuteChanged -= this._commandEvent;
            this._commandEvent = null;
            this._UpdateCanExecute();
        }

        private void _UpdateCanExecute()
        {
            if (this.Command != null)
            {
                object commandParameter = this.CommandParameter;
                IInputElement commandTarget = this.CommandTarget;
                RoutedCommand command = this.Command as RoutedCommand;
                if (command != null)
                {
                    this._CanExecute = command.CanExecute(commandParameter, commandTarget);
                }
                else
                {
                    this._CanExecute = this.Command.CanExecute(commandParameter);
                }
            }
            else
            {
                this._CanExecute = true;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new ThumbButtonInfo();
        }

        internal void InvokeClick()
        {
            EventHandler click = this.Click;
            if (click != null)
            {
                click(this, EventArgs.Empty);
            }
            this._InvokeCommand();
        }

        private bool _CanExecute
        {
            get
            {
                return (bool) base.GetValue(_CanExecuteProperty);
            }
            set
            {
                base.SetValue(_CanExecuteProperty, value);
            }
        }

        public ICommand Command
        {
            get
            {
                return (ICommand) base.GetValue(CommandProperty);
            }
            set
            {
                base.SetValue(CommandProperty, value);
            }
        }

        public object CommandParameter
        {
            get
            {
                return base.GetValue(CommandParameterProperty);
            }
            set
            {
                base.SetValue(CommandParameterProperty, value);
            }
        }

        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement) base.GetValue(CommandTargetProperty);
            }
            set
            {
                base.SetValue(CommandTargetProperty, value);
            }
        }

        public string Description
        {
            get
            {
                return (string) base.GetValue(DescriptionProperty);
            }
            set
            {
                base.SetValue(DescriptionProperty, value);
            }
        }

        public bool DismissWhenClicked
        {
            get
            {
                return (bool) base.GetValue(DismissWhenClickedProperty);
            }
            set
            {
                base.SetValue(DismissWhenClickedProperty, value);
            }
        }

        public System.Windows.Media.ImageSource ImageSource
        {
            get
            {
                return (System.Windows.Media.ImageSource) base.GetValue(ImageSourceProperty);
            }
            set
            {
                base.SetValue(ImageSourceProperty, value);
            }
        }

        public bool IsBackgroundVisible
        {
            get
            {
                return (bool) base.GetValue(IsBackgroundVisibleProperty);
            }
            set
            {
                base.SetValue(IsBackgroundVisibleProperty, value);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return (bool) base.GetValue(IsEnabledProperty);
            }
            set
            {
                base.SetValue(IsEnabledProperty, value);
            }
        }

        public bool IsInteractive
        {
            get
            {
                return (bool) base.GetValue(IsInteractiveProperty);
            }
            set
            {
                base.SetValue(IsInteractiveProperty, value);
            }
        }

        public System.Windows.Visibility Visibility
        {
            get
            {
                return (System.Windows.Visibility) base.GetValue(VisibilityProperty);
            }
            set
            {
                base.SetValue(VisibilityProperty, value);
            }
        }
    }
}

