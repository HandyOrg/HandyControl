using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using HandyControl.Tools.Interop;

namespace HandyControl.Controls;

/// <summary>
/// Displays a message box with text, buttons, and symbols to inform and instruct the user.
/// </summary>
[SuppressMessage("ReSharper", "RedundantDelegateCreation")]
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
[TemplatePart(Name = ElementButtonClose, Type = typeof(Button))]
public sealed class MessageBox : Window
{
    private const string ElementPanel = "PART_Panel";
    private const string ElementButtonClose = "PART_ButtonClose";

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        nameof(Message), typeof(string), typeof(MessageBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
        nameof(Image), typeof(Geometry), typeof(MessageBox), new PropertyMetadata(default(Geometry)));

    public static readonly DependencyProperty ImageBrushProperty = DependencyProperty.Register(
        nameof(ImageBrush), typeof(Brush), typeof(MessageBox), new PropertyMetadata(default(Brush)));

    public static readonly DependencyProperty ShowImageProperty = DependencyProperty.Register(
        nameof(ShowImage), typeof(bool), typeof(MessageBox), new PropertyMetadata(ValueBoxes.FalseBox));

    private Button _buttonClose;
    private Panel _panel;
    private MessageBoxResult _messageBoxResult = MessageBoxResult.Cancel;
    private Button _buttonOk;
    private Button _buttonCancel;
    private Button _buttonYes;
    private Button _buttonNo;
    private bool _showOk;
    private bool _showCancel;
    private bool _showYes;
    private bool _showNo;
    private IntPtr _lastActiveWindowIntPtr;

    /// <summary>
    /// This constructor is private so people aren't tempted to try and create instances of these -- they should just use the static methods.
    /// </summary>
    private MessageBox()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, (s, e) =>
        {
            _messageBoxResult = MessageBoxResult.OK;
            Close();
        }, (s, e) => e.CanExecute = _showOk));
        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, (s, e) =>
        {
            _messageBoxResult = MessageBoxResult.Cancel;
            Close();
        }, (s, e) => e.CanExecute = _showCancel));
        CommandBindings.Add(new CommandBinding(ControlCommands.Yes, (s, e) =>
        {
            _messageBoxResult = MessageBoxResult.Yes;
            Close();
        }, (s, e) => e.CanExecute = _showYes));
        CommandBindings.Add(new CommandBinding(ControlCommands.No, (s, e) =>
        {
            _messageBoxResult = MessageBoxResult.No;
            Close();
        }, (s, e) => e.CanExecute = _showNo));
    }

    public string Message
    {
        get => (string) GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public Geometry Image
    {
        get => (Geometry) GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public Brush ImageBrush
    {
        get => (Brush) GetValue(ImageBrushProperty);
        set => SetValue(ImageBrushProperty, value);
    }

    public bool ShowImage
    {
        get => (bool) GetValue(ShowImageProperty);
        set => SetValue(ShowImageProperty, ValueBoxes.BooleanBox(value));
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        if (_showYes && !_showCancel)
        {
            var hMenu = InteropMethods.GetSystemMenu(this.GetHandle(), false);
            if (hMenu != IntPtr.Zero)
            {
                InteropMethods.EnableMenuItem(hMenu, InteropValues.SC_CLOSE,
                    InteropValues.MF_BYCOMMAND | InteropValues.MF_GRAYED);
            }

            if (_buttonClose != null)
            {
                _buttonClose.IsEnabled = false;
            }
        }

        base.OnSourceInitialized(e);

        _lastActiveWindowIntPtr = InteropMethods.GetForegroundWindow();
        Activate();
    }

    protected override void OnClosed(EventArgs e)
    {
        InteropMethods.SetForegroundWindow(_lastActiveWindowIntPtr);

        base.OnClosed(e);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _panel = GetTemplateChild(ElementPanel) as Panel;
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

        _buttonClose = GetTemplateChild(ElementButtonClose) as Button;
        if (_buttonClose != null)
        {
            _buttonClose.Click += ButtonClose_Click;
        }
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e) => Close();

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (e is { Key: Key.System, SystemKey: Key.F4 })
        {
            e.Handled = true;
            return;
        }

        if (e.KeyboardDevice.Modifiers != ModifierKeys.Control || e.Key != Key.C)
        {
            return;
        }

        const int dashCount = 27;
        var line = new string('-', dashCount);

        var builder = new StringBuilder()
            .Append(line)
            .Append(Environment.NewLine)
            .Append(Title)
            .Append(Environment.NewLine)
            .Append(line)
            .Append(Environment.NewLine)
            .Append(Message)
            .Append(Environment.NewLine)
            .Append(line)
            .Append(Environment.NewLine);

        if (_showOk)
        {
            builder.Append(Lang.Confirm);
            builder.Append("   ");
        }

        if (_showYes)
        {
            builder.Append(Lang.Yes);
            builder.Append("   ");
        }

        if (_showNo)
        {
            builder.Append(Lang.No);
            builder.Append("   ");
        }

        if (_showCancel)
        {
            builder.Append(Lang.Cancel);
            builder.Append("   ");
        }

        builder.Append(Environment.NewLine);
        builder.Append(line);
        builder.Append(Environment.NewLine);

        try
        {
            Clipboard.SetDataObject(builder.ToString());
        }
        catch
        {
            // ignored
        }
    }

    public static MessageBoxResult Success(string messageBoxText, string caption = null)
    {
        return Success(owner: null, messageBoxText, caption);
    }

    public static MessageBoxResult Success(System.Windows.Window owner, string messageBoxText, string caption = null)
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
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
            messageBox.Image = ResourceHelper.GetResourceInternal<Geometry>(ResourceToken.SuccessGeometry);
            messageBox.ImageBrush = ResourceHelper.GetResourceInternal<Brush>(ResourceToken.SuccessBrush);
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    public static MessageBoxResult Info(string messageBoxText, string caption = null)
    {
        return Info(owner: null, messageBoxText, caption);
    }

    public static MessageBoxResult Info(System.Windows.Window owner, string messageBoxText, string caption = null)
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
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
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    public static MessageBoxResult Warning(string messageBoxText, string caption = null)
    {
        return Warning(owner: null, messageBoxText, caption);
    }

    public static MessageBoxResult Warning(System.Windows.Window owner, string messageBoxText, string caption = null)
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
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
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    public static MessageBoxResult Error(string messageBoxText, string caption = null)
    {
        return Error(owner: null, messageBoxText, caption);
    }

    public static MessageBoxResult Error(System.Windows.Window owner, string messageBoxText, string caption = null)
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
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
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    public static MessageBoxResult Fatal(string messageBoxText, string caption = null)
    {
        return Fatal(owner: null, messageBoxText, caption);
    }

    public static MessageBoxResult Fatal(System.Windows.Window owner, string messageBoxText, string caption = null)
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
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
            messageBox.Image = ResourceHelper.GetResourceInternal<Geometry>(ResourceToken.FatalGeometry);
            messageBox.ImageBrush = ResourceHelper.GetResourceInternal<Brush>(ResourceToken.PrimaryTextBrush);
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    public static MessageBoxResult Ask(string messageBoxText, string caption = null)
    {
        return Ask(owner: null, messageBoxText, caption);
    }

    public static MessageBoxResult Ask(System.Windows.Window owner, string messageBoxText, string caption = null)
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
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
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    public static MessageBoxResult Show(MessageBoxInfo info)
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
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

            if (info.StyleKey != null)
            {
                messageBox.Style = ResourceHelper.GetResource<Style>(info.StyleKey) ?? info.Style;
            }

            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    public static MessageBoxResult Show(
        string messageBoxText,
        string caption = null,
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.None,
        MessageBoxResult defaultResult = MessageBoxResult.None
    )
    {
        return Show(owner: null, messageBoxText, caption, button, icon, defaultResult);
    }

    public static MessageBoxResult Show(
        System.Windows.Window owner,
        string messageBoxText,
        string caption = null,
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.None,
        MessageBoxResult defaultResult = MessageBoxResult.None
    )
    {
        MessageBox messageBox = null;
        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            messageBox = CreateMessageBox(owner, messageBoxText, caption, button, icon, defaultResult);
            SetButtonStatus(messageBox, button, defaultResult);
            SetImage(messageBox, icon);
            SystemSounds.Asterisk.Play();
            messageBox.ShowDialog();
        }));

        return messageBox._messageBoxResult;
    }

    private static MessageBox CreateMessageBox(
        System.Windows.Window owner,
        string messageBoxText,
        string caption,
        MessageBoxButton button,
        MessageBoxImage icon,
        MessageBoxResult defaultResult
    )
    {
        if (!IsValidMessageBoxButton(button))
        {
            throw new InvalidEnumArgumentException(nameof(button), (int) button, typeof(MessageBoxButton));
        }

        if (!IsValidMessageBoxImage(icon))
        {
            throw new InvalidEnumArgumentException(nameof(icon), (int) icon, typeof(MessageBoxImage));
        }

        if (!IsValidMessageBoxResult(defaultResult))
        {
            throw new InvalidEnumArgumentException(nameof(defaultResult), (int) defaultResult,
                typeof(MessageBoxResult));
        }

        var ownerWindow = owner ?? WindowHelper.GetActiveWindow();
        var ownerIsNull = ownerWindow is null;

        return new MessageBox
        {
            Message = messageBoxText,
            Owner = ownerWindow,
            WindowStartupLocation =
                ownerIsNull ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner,
            ShowTitle = true,
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
                messageBox._showOk = true;
                messageBox._buttonOk = new Button
                {
                    IsCancel = true,
                    IsDefault = true,
                    Content = Lang.Confirm,
                    Command = ControlCommands.Confirm,
                    Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle")
                };

                break;
            case MessageBoxButton.OKCancel:
                messageBox._messageBoxResult = MessageBoxResult.Cancel;
                messageBox._showOk = true;
                messageBox._buttonOk = new Button
                {
                    Content = Lang.Confirm,
                    Command = ControlCommands.Confirm
                };

                messageBox._showCancel = true;
                messageBox._buttonCancel = new Button
                {
                    IsCancel = true,
                    Content = Lang.Cancel,
                    Command = ControlCommands.Cancel
                };

                if (defaultResult == MessageBoxResult.Cancel)
                {
                    messageBox._buttonOk.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                    messageBox._buttonCancel.Style =
                        ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
                    messageBox._buttonCancel.IsDefault = true;
                }
                else
                {
                    messageBox._buttonOk.Style =
                        ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
                    messageBox._buttonOk.IsDefault = true;
                    messageBox._buttonCancel.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                }

                break;
            case MessageBoxButton.YesNo:
                messageBox._messageBoxResult = MessageBoxResult.Cancel;
                messageBox._showYes = true;
                messageBox._buttonYes = new Button
                {
                    Content = Lang.Yes,
                    Command = ControlCommands.Yes
                };

                messageBox._showNo = true;
                messageBox._buttonNo = new Button
                {
                    Content = Lang.No,
                    Command = ControlCommands.No
                };

                if (defaultResult == MessageBoxResult.No)
                {
                    messageBox._buttonYes.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                    messageBox._buttonNo.Style =
                        ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
                    messageBox._buttonNo.IsDefault = true;
                }
                else
                {
                    messageBox._buttonYes.Style =
                        ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
                    messageBox._buttonYes.IsDefault = true;
                    messageBox._buttonNo.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                }

                break;
            case MessageBoxButton.YesNoCancel:
                messageBox._messageBoxResult = MessageBoxResult.Cancel;
                messageBox._showYes = true;
                messageBox._buttonYes = new Button
                {
                    Content = Lang.Yes,
                    Command = ControlCommands.Yes
                };

                messageBox._showNo = true;
                messageBox._buttonNo = new Button
                {
                    Content = Lang.No,
                    Command = ControlCommands.No
                };

                messageBox._showCancel = true;
                messageBox._buttonCancel = new Button
                {
                    IsCancel = true,
                    Content = Lang.Cancel,
                    Command = ControlCommands.Cancel
                };

                if (defaultResult == MessageBoxResult.No)
                {
                    messageBox._buttonYes.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                    messageBox._buttonNo.Style =
                        ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
                    messageBox._buttonNo.IsDefault = true;
                    messageBox._buttonCancel.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                }
                else if (defaultResult == MessageBoxResult.Cancel)
                {
                    messageBox._buttonYes.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                    messageBox._buttonNo.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                    messageBox._buttonCancel.Style =
                        ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
                    messageBox._buttonCancel.IsDefault = true;
                }
                else
                {
                    messageBox._buttonYes.Style =
                        ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
                    messageBox._buttonYes.IsDefault = true;
                    messageBox._buttonNo.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
                    messageBox._buttonCancel.Style = ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
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
        messageBox.Image = ResourceHelper.GetResourceInternal<Geometry>(iconKey);
        messageBox.ImageBrush = ResourceHelper.GetResourceInternal<Brush>(iconBrushKey);
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
