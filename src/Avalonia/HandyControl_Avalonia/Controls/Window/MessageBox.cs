using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using HandyControl.Data;
using HandyControl.Properties.Langs;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    /// <summary>
    /// Displays a message box with text, buttons, and symbols to inform and instruct the user.
    /// </summary>
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    [TemplatePart(Name = ElementButtonClose, Type = typeof(Button))]
    public sealed class MessageBox : HandyControl.Controls.Window
    {
        private const string ElementPanel = "PART_Panel";
        private const string ElementButtonClose = "PART_ButtonClose";
        private const string ElementButtonIcon = "PART_ButtonIcon";

        public static readonly StyledProperty<string> MessageProperty = AvaloniaProperty.Register<MessageBox, string>(
            nameof(Message));

        public static readonly StyledProperty<Geometry> ImageProperty = AvaloniaProperty.Register<MessageBox, Geometry>(
            nameof(Image));
        public static readonly StyledProperty<Brush> ImageBrushProperty = AvaloniaProperty.Register<MessageBox, Brush>(
            nameof(ImageBrush));

        public static readonly StyledProperty<bool> ShowImageProperty = AvaloniaProperty.Register<MessageBox, bool>(
            nameof(ShowImage));

        private Button _buttonClose;
        private Panel _panel;
        private MessageBoxResult _messageBoxResult = MessageBoxResult.Cancel;
        private Button _buttonOk;
        private Button _buttonCancel;
        private Button _buttonYes;
        private Button _buttonNo;

        private EventHandler<RoutedEventArgs> _ConfirmCommand;
        private EventHandler<RoutedEventArgs> _CancelCommand;
        private EventHandler<RoutedEventArgs> _YesCommand;
        private EventHandler<RoutedEventArgs> _NoCommand;

        static MessageBox()
        {
            ThemeProperty.OverrideDefaultValue<MessageBox>(ResourceHelper.GetResource<ControlTheme>("DefaultMessageBoxStyle"));
        }

        /// <summary>
        /// This constructor is private so people aren't tempted to try and create instances of these -- they should just use the static methods.
        /// </summary>
        private MessageBox()
        {
            _ConfirmCommand = (_, __) =>
            {
                _messageBoxResult = MessageBoxResult.OK;
                Close();
            };
            _CancelCommand = (_, __) =>
            {
                _messageBoxResult = MessageBoxResult.Cancel;
                Close();
            };
            _YesCommand = (_, __) =>
            {
                _messageBoxResult = MessageBoxResult.Yes;
                Close();
            };
            _NoCommand = (_, __) =>
            {
                _messageBoxResult = MessageBoxResult.No;
                Close();
            };
        }

        public string Message
        {
            get => GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public Geometry Image
        {
            get => GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public Brush ImageBrush
        {
            get => GetValue(ImageBrushProperty);
            set => SetValue(ImageBrushProperty, value);
        }

        public bool ShowImage
        {
            get => GetValue(ShowImageProperty);
            set => SetValue(ShowImageProperty, ValueBoxes.BooleanBox(value));
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _panel = e.NameScope.Find<Panel>(ElementPanel);
            if (_panel != null)
            {
                if (_buttonOk != null)
                {
                    _panel.Children.Add(_buttonOk);
                }

                if (_buttonYes != null)
                {
                    _panel.Children.Add(_buttonYes);
                }

                if (_buttonNo != null)
                {
                    _panel.Children.Add(_buttonNo);
                }

                if (_buttonCancel != null)
                {
                    _panel.Children.Add(_buttonCancel);
                }
            }

            _buttonClose = e.NameScope.Find<Button>(ElementButtonClose);
            if (_buttonClose != null)
            {
                _buttonClose.Click += ButtonClose_Click;
            }

            var buttonIcon = e.NameScope.Find<Button>(ElementButtonIcon);
            if (buttonIcon != null && buttonIcon.IsVisible)
            {
                buttonIcon.Click += ButtonClose_Click;
            }
        }

        private void ButtonClose_Click(object? sender, RoutedEventArgs e) => Close();


        public static async Task<MessageBoxResult> Success(string messageBoxText, string caption = null)
        {
            return await Success(owner: null, messageBoxText, caption);
        }

        public static async Task<MessageBoxResult> Success(Avalonia.Controls.Window owner, string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(
                    owner: owner,
                    messageBoxText: messageBoxText,
                    caption: caption,
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.None,
                    defaultResult: MessageBoxResult.OK
                );
                SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
                messageBox.ShowImage = true;
                messageBox.Image = ResourceHelper.GetResource<Geometry>(ResourceToken.SuccessGeometry);
                messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(ResourceToken.SuccessBrush);

                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        public static async Task<MessageBoxResult> Info(string messageBoxText, string caption = null)
        {
            return await Info(owner: null, messageBoxText, caption);
        }

        public static async Task<MessageBoxResult> Info(Avalonia.Controls.Window owner, string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(
                    owner: owner,
                    messageBoxText: messageBoxText,
                    caption: caption,
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Information,
                    defaultResult: MessageBoxResult.OK
                );
                SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
                SetImage(messageBox, MessageBoxImage.Information);
                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        public static async Task<MessageBoxResult> Warning(string messageBoxText, string caption = null)
        {
            return await Warning(owner: null, messageBoxText, caption);
        }

        public static async Task<MessageBoxResult> Warning(Avalonia.Controls.Window owner, string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(
                    owner: owner,
                    messageBoxText: messageBoxText,
                    caption: caption,
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Warning,
                    defaultResult: MessageBoxResult.OK
                );
                SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
                SetImage(messageBox, MessageBoxImage.Warning);
                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        public static async Task<MessageBoxResult> Error(string messageBoxText, string caption = null)
        {
            return await Error(owner: null, messageBoxText, caption);
        }

        public static async Task<MessageBoxResult> Error(Avalonia.Controls.Window owner, string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(
                    owner: owner,
                    messageBoxText: messageBoxText,
                    caption: caption,
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error,
                    defaultResult: MessageBoxResult.OK
                );
                SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
                SetImage(messageBox, MessageBoxImage.Error);
                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        public static async Task<MessageBoxResult> Fatal(string messageBoxText, string caption = null)
        {
            return await Fatal(owner: null, messageBoxText, caption);
        }

        public static async Task<MessageBoxResult> Fatal(Avalonia.Controls.Window owner, string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(
                    owner: owner,
                    messageBoxText: messageBoxText,
                    caption: caption,
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.None,
                    defaultResult: MessageBoxResult.OK
                );
                SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
                messageBox.ShowImage = true;
                messageBox.Image = ResourceHelper.GetResource<Geometry>(ResourceToken.FatalGeometry);
                messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(ResourceToken.PrimaryTextBrush);
                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        public static async Task<MessageBoxResult> Ask(string messageBoxText, string caption = null)
        {
            return await Ask(owner: null, messageBoxText, caption);
        }

        public static async Task<MessageBoxResult> Ask(Avalonia.Controls.Window owner, string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(
                    owner: owner,
                    messageBoxText: messageBoxText,
                    caption: caption,
                    button: MessageBoxButton.OKCancel,
                    icon: MessageBoxImage.Question,
                    defaultResult: MessageBoxResult.Cancel
                );
                SetButtonStatus(messageBox, MessageBoxButton.OKCancel, MessageBoxResult.Cancel);
                SetImage(messageBox, MessageBoxImage.Question);
                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        public static async Task<MessageBoxResult> Show(MessageBoxInfo info)
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(
                    owner: info.Owner,
                    messageBoxText: info.Message,
                    caption: info.Caption,
                    button: info.Button,
                    icon: MessageBoxImage.None,
                    defaultResult: info.DefaultResult
                );
                SetButtonStatus(messageBox, info.Button, info.DefaultResult);

                if (!string.IsNullOrEmpty(info.IconKey))
                {
                    messageBox.ShowImage = true;
                    messageBox.Image = ResourceHelper.GetResource<Geometry>(info.IconKey) ?? info.Icon;
                    messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(info.IconBrushKey) ?? info.IconBrush;
                }

                if (info.ControlThemeKey != null)
                {
                    messageBox.Theme = ResourceHelper.GetResource<ControlTheme>(info.ControlThemeKey) ?? info.ControlTheme;
                }
                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        public static async Task<MessageBoxResult> Show(
            string messageBoxText,
            string caption = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.None
        )
        {
            return await Show(owner: null, messageBoxText, caption, button, icon, defaultResult);
        }


        public static async Task<MessageBoxResult> Show(
            Avalonia.Controls.Window owner,
            string messageBoxText,
            string caption = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.None
        )
        {
            MessageBox messageBox = null;
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                messageBox = CreateMessageBox(owner, messageBoxText, caption, button, icon, defaultResult);
                SetButtonStatus(messageBox, button, defaultResult);
                SetImage(messageBox, icon);
                await messageBox.ShowDialog(messageBox.Owner as Avalonia.Controls.Window);
            });

            return messageBox._messageBoxResult;
        }

        private static MessageBox CreateMessageBox(
            Avalonia.Controls.Window owner,
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult
        )
        {
            if (!IsValidMessageBoxButton(button))
            {
                throw new InvalidEnumArgumentException(nameof(button), (int)button, typeof(MessageBoxButton));
            }

            if (!IsValidMessageBoxImage(icon))
            {
                throw new InvalidEnumArgumentException(nameof(icon), (int)icon, typeof(MessageBoxImage));
            }

            if (!IsValidMessageBoxResult(defaultResult))
            {
                throw new InvalidEnumArgumentException(nameof(defaultResult), (int)defaultResult,
                    typeof(MessageBoxResult));
            }
            Avalonia.Controls.Window ownerWindow = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ownerWindow = desktop.Windows.FirstOrDefault(w => w.IsActive);
            }
            var ownerIsNull = ownerWindow is null;

            return new MessageBox
            {
                Message = messageBoxText,
                Owner = ownerWindow,
                WindowStartupLocation = ownerIsNull ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner,
                Title = caption ?? string.Empty,
                Topmost = ownerIsNull,
                _messageBoxResult = defaultResult
            };
        }

        private static void SetButtonStatus(
            MessageBox messageBox,
            MessageBoxButton messageBoxButton,
            MessageBoxResult defaultResult
        )
        {
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    messageBox._messageBoxResult = MessageBoxResult.Yes;
                    messageBox._buttonOk = new Button
                    {
                        IsCancel = true,
                        IsDefault = true,
                        Content = Lang.Confirm,
                        Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle")
                    };
                    messageBox._buttonOk.Click += messageBox._ConfirmCommand;
                    break;
                case MessageBoxButton.OKCancel:
                    messageBox._messageBoxResult = MessageBoxResult.Cancel;
                    messageBox._buttonOk = new Button
                    {
                        Content = Lang.Confirm,
                    };
                    messageBox._buttonOk.Click += messageBox._ConfirmCommand;
                    messageBox._buttonCancel = new Button
                    {
                        IsCancel = true,
                        Content = Lang.Cancel,
                    };
                    messageBox._buttonCancel.Click += messageBox._CancelCommand;
                    if (defaultResult == MessageBoxResult.Cancel)
                    {
                        messageBox._buttonOk.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                        messageBox._buttonCancel.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxPrimaryButtonStyle");
                        messageBox._buttonCancel.IsDefault = true;
                    }
                    else
                    {
                        messageBox._buttonOk.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxPrimaryButtonStyle");
                        messageBox._buttonOk.IsDefault = true;
                        messageBox._buttonCancel.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                    }

                    break;
                case MessageBoxButton.YesNo:
                    messageBox._messageBoxResult = MessageBoxResult.Cancel;
                    messageBox._buttonYes = new Button
                    {
                        Content = Lang.Yes,
                    };
                    messageBox._buttonYes.Click += messageBox._YesCommand;
                    messageBox._buttonNo = new Button
                    {
                        Content = Lang.No,
                    };
                    messageBox._buttonNo.Click += messageBox._NoCommand;
                    if (defaultResult == MessageBoxResult.No)
                    {
                        messageBox._buttonYes.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                        messageBox._buttonNo.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxPrimaryButtonStyle");
                        messageBox._buttonNo.IsDefault = true;
                    }
                    else
                    {
                        messageBox._buttonYes.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxPrimaryButtonStyle");
                        messageBox._buttonYes.IsDefault = true;
                        messageBox._buttonNo.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                    }

                    break;
                case MessageBoxButton.YesNoCancel:
                    messageBox._messageBoxResult = MessageBoxResult.Cancel;
                    messageBox._buttonYes = new Button
                    {
                        Content = Lang.Yes,
                    };
                    messageBox._buttonYes.Click += messageBox._YesCommand;
                    messageBox._buttonNo = new Button
                    {
                        Content = Lang.No,
                    };
                    messageBox._buttonNo.Click += messageBox._NoCommand;
                    messageBox._buttonCancel = new Button
                    {
                        IsCancel = true,
                        Content = Lang.Cancel,
                    };
                    messageBox._buttonCancel.Click += messageBox._CancelCommand;

                    if (defaultResult == MessageBoxResult.No)
                    {
                        messageBox._buttonYes.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                        messageBox._buttonNo.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxPrimaryButtonStyle");
                        messageBox._buttonNo.IsDefault = true;
                        messageBox._buttonCancel.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                    }
                    else if (defaultResult == MessageBoxResult.Cancel)
                    {
                        messageBox._buttonYes.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                        messageBox._buttonNo.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                        messageBox._buttonCancel.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxPrimaryButtonStyle");
                        messageBox._buttonCancel.IsDefault = true;
                    }
                    else
                    {
                        messageBox._buttonYes.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxPrimaryButtonStyle");
                        messageBox._buttonYes.IsDefault = true;
                        messageBox._buttonNo.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                        messageBox._buttonCancel.Theme = ResourceHelper.GetResource<ControlTheme>("MessageBoxButtonStyle");
                    }

                    break;
            }
        }


        private static void SetImage(MessageBox messageBox, MessageBoxImage messageBoxImage)
        {
            var iconKey = string.Empty;
            var iconBrushKey = string.Empty;

            switch (messageBoxImage)
            {
                case MessageBoxImage.Error:
                    iconKey = ResourceToken.ErrorGeometry;
                    iconBrushKey = ResourceToken.DangerBrush;
                    break;
                case MessageBoxImage.Question:
                    iconKey = ResourceToken.AskGeometry;
                    iconBrushKey = ResourceToken.AccentBrush;
                    break;
                case MessageBoxImage.Warning:
                    iconKey = ResourceToken.WarningGeometry;
                    iconBrushKey = ResourceToken.WarningBrush;
                    break;
                case MessageBoxImage.Information:
                    iconKey = ResourceToken.InfoGeometry;
                    iconBrushKey = ResourceToken.InfoBrush;
                    break;
            }

            if (string.IsNullOrEmpty(iconKey))
            {
                return;
            }

            messageBox.ShowImage = true;
            messageBox.Image = ResourceHelper.GetResource<Geometry>(iconKey);
            messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(iconBrushKey);
        }

        private static bool IsValidMessageBoxButton(MessageBoxButton value)
        {
            return value
                is MessageBoxButton.OK
                or MessageBoxButton.OKCancel
                or MessageBoxButton.YesNo
                or MessageBoxButton.YesNoCancel;
        }

        private static bool IsValidMessageBoxImage(MessageBoxImage value)
        {
            return value
                is MessageBoxImage.Asterisk
                or MessageBoxImage.Error
                or MessageBoxImage.Exclamation
                or MessageBoxImage.Hand
                or MessageBoxImage.Information
                or MessageBoxImage.None
                or MessageBoxImage.Question
                or MessageBoxImage.Stop
                or MessageBoxImage.Warning;
        }

        private static bool IsValidMessageBoxResult(MessageBoxResult value)
        {
            return value
                is MessageBoxResult.Cancel
                or MessageBoxResult.No
                or MessageBoxResult.None
                or MessageBoxResult.OK
                or MessageBoxResult.Yes;
        }

    }

}
