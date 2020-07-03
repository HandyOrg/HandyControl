using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    /// <summary>
    ///     消息框
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantDelegateCreation")]
    public sealed class MessageBox : Window
    {
        private MessageBoxResult _messageBoxResult = MessageBoxResult.Cancel;

        private bool _showOk;

        private bool _showCancel;

        private bool _showYes;

        private bool _showNo;

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(MessageBox), new PropertyMetadata(default(string)));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(Geometry), typeof(MessageBox), new PropertyMetadata(default(Geometry)));

        public Geometry Image
        {
            get => (Geometry)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageBrushProperty = DependencyProperty.Register(
            "ImageBrush", typeof(Brush), typeof(MessageBox), new PropertyMetadata(default(Brush)));

        public Brush ImageBrush
        {
            get => (Brush)GetValue(ImageBrushProperty);
            set => SetValue(ImageBrushProperty, value);
        }

        public static readonly DependencyProperty ShowImageProperty = DependencyProperty.Register(
            "ShowImage", typeof(bool), typeof(MessageBox), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowImage
        {
            get => (bool)GetValue(ShowImageProperty);
            set => SetValue(ShowImageProperty, ValueBoxes.BooleanBox(value));
        }

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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                var builder = new StringBuilder();
                var line = new string('-', 27);
                builder.Append(line);
                builder.Append(Environment.NewLine);
                builder.Append(Title);
                builder.Append(Environment.NewLine);
                builder.Append(line);
                builder.Append(Environment.NewLine);
                builder.Append(Message);
                builder.Append(Environment.NewLine);
                builder.Append(line);
                builder.Append(Environment.NewLine);
                if (_showOk)
                {
                    builder.Append(Properties.Langs.Lang.Confirm);
                    builder.Append("   ");
                }
                if (_showYes)
                {
                    builder.Append(Properties.Langs.Lang.Yes);
                    builder.Append("   ");
                }
                if (_showNo)
                {
                    builder.Append(Properties.Langs.Lang.No);
                    builder.Append("   ");
                }
                if (_showCancel)
                {
                    builder.Append(Properties.Langs.Lang.Cancel);
                    builder.Append("   ");
                }
                builder.Append(Environment.NewLine);
                builder.Append(line);
                builder.Append(Environment.NewLine);
                Clipboard.SetText(builder.ToString());
            }
        }

        /// <summary>
        ///     成功
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        public static MessageBoxResult Success(string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                messageBox.ShowImage = true;
                messageBox.Image = ResourceHelper.GetResource<Geometry>(ResourceToken.SuccessGeometry);
                messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(ResourceToken.SuccessBrush);
                SystemSounds.Asterisk.Play();
                messageBox.ShowDialog();
            }));

            return messageBox._messageBoxResult;
        }

        /// <summary>
        ///     消息
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        public static MessageBoxResult Info(string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                SetImage(messageBox, MessageBoxImage.Information);
                SystemSounds.Asterisk.Play();
                messageBox.ShowDialog();
            }));

            return messageBox._messageBoxResult;
        }

        /// <summary>
        ///     警告
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        public static MessageBoxResult Warning(string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                SetImage(messageBox, MessageBoxImage.Warning);
                SystemSounds.Asterisk.Play();
                messageBox.ShowDialog();
            }));

            return messageBox._messageBoxResult;
        }

        /// <summary>
        ///     错误
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        public static MessageBoxResult Error(string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                SetImage(messageBox, MessageBoxImage.Error);
                SystemSounds.Asterisk.Play();
                messageBox.ShowDialog();
            }));

            return messageBox._messageBoxResult;
        }

        /// <summary>
        ///     严重
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        public static MessageBoxResult Fatal(string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                messageBox.ShowImage = true;
                messageBox.Image = ResourceHelper.GetResource<Geometry>(ResourceToken.FatalGeometry);
                messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(ResourceToken.PrimaryTextBrush);
                SystemSounds.Asterisk.Play();
                messageBox.ShowDialog();
            }));

            return messageBox._messageBoxResult;
        }

        /// <summary>
        ///     询问
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        public static MessageBoxResult Ask(string messageBoxText, string caption = null)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
                SetButtonStatus(messageBox, MessageBoxButton.OKCancel);
                SetImage(messageBox, MessageBoxImage.Question);
                SystemSounds.Asterisk.Play();
                messageBox.ShowDialog();
            }));

            return messageBox._messageBoxResult;
        }

        /// <summary>
        ///     自定义信息展示
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(MessageBoxInfo info)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(null, info.Message, info.Caption, info.Button, MessageBoxImage.None, info.DefaultResult);
                SetButtonStatus(messageBox, info.Button);

                if (!string.IsNullOrEmpty(info.IconKey))
                {
                    messageBox.ShowImage = true;
                    messageBox.Image = ResourceHelper.GetResource<Geometry>(info.IconKey);
                    messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(info.IconBrushKey);
                }

                if (info.StyleKey != null)
                {
                    messageBox.Style = ResourceHelper.GetResource<Style>(info.StyleKey);
                }
                SystemSounds.Asterisk.Play();
                messageBox.ShowDialog();
            }));

            return messageBox._messageBoxResult;
        }

        /// <summary>
        ///     信息展示
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(string messageBoxText, string caption = null,
            MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
            MessageBoxResult defaultResult = MessageBoxResult.None) =>
            Show(null, messageBoxText, caption, button, icon, defaultResult);

        /// <summary>
        ///     信息展示
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(System.Windows.Window owner, string messageBoxText, string caption = null, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                messageBox = CreateMessageBox(owner, messageBoxText, caption, button, icon, defaultResult);
                SetButtonStatus(messageBox, button);
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
            MessageBoxResult defaultResult)
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
                throw new InvalidEnumArgumentException(nameof(defaultResult), (int)defaultResult, typeof(MessageBoxResult));
            }

            var ownerWindow = owner ?? WindowHelper.GetActiveWindow();
            var ownerIsNull = ownerWindow is null;

            return new MessageBox
            {
                Message = messageBoxText,
                Owner = ownerWindow,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowTitle = true,
                Title = caption ?? string.Empty,
                Topmost = ownerIsNull,
                _messageBoxResult = defaultResult
            };
        }

        private static void SetButtonStatus(MessageBox messageBox, MessageBoxButton messageBoxButton)
        {
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    messageBox._showOk = true;
                    break;
                case MessageBoxButton.OKCancel:
                    messageBox._showOk = true;
                    messageBox._showCancel = true;
                    break;
                case MessageBoxButton.YesNo:
                    messageBox._showYes = true;
                    messageBox._showNo = true;
                    break;
                case MessageBoxButton.YesNoCancel:
                    messageBox._showYes = true;
                    messageBox._showNo = true;
                    messageBox._showCancel = true;
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

            if (string.IsNullOrEmpty(iconKey)) return;
            messageBox.ShowImage = true;
            messageBox.Image = ResourceHelper.GetResource<Geometry>(iconKey);
            messageBox.ImageBrush = ResourceHelper.GetResource<Brush>(iconBrushKey);
        }

        private static bool IsValidMessageBoxButton(MessageBoxButton value)
        {
            return value == MessageBoxButton.OK
                   || value == MessageBoxButton.OKCancel
                   || value == MessageBoxButton.YesNo
                   || value == MessageBoxButton.YesNoCancel;
        }

        private static bool IsValidMessageBoxImage(MessageBoxImage value)
        {
            return value == MessageBoxImage.Asterisk
                   || value == MessageBoxImage.Error
                   || value == MessageBoxImage.Exclamation
                   || value == MessageBoxImage.Hand
                   || value == MessageBoxImage.Information
                   || value == MessageBoxImage.None
                   || value == MessageBoxImage.Question
                   || value == MessageBoxImage.Stop
                   || value == MessageBoxImage.Warning;
        }

        private static bool IsValidMessageBoxResult(MessageBoxResult value)
        {
            return value == MessageBoxResult.Cancel
                   || value == MessageBoxResult.No
                   || value == MessageBoxResult.None
                   || value == MessageBoxResult.OK
                   || value == MessageBoxResult.Yes;
        }
    }
}