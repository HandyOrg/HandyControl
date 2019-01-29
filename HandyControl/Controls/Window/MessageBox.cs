using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    /// <summary>
    ///     消息框
    /// </summary>
    public sealed class MessageBox : Window
    {
        #region Constants

        private const string ElementImage = "PART_Image";

        private const string ElementMessage = "PART_Message";

        #endregion Constants

        private MessageBoxResult _messageBoxResult = MessageBoxResult.Cancel;

        private bool _showOk;

        private bool _showCancel;

        private bool _showYes;

        private bool _showNo;

        private string _message;

        private Geometry _image;

        private Brush _imageBrush;

        private bool _showImage;

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

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (LogicalTreeHelper.FindLogicalNode(this, ElementMessage) is TextBlock message) message.Text = _message;

            if (_showImage)
            {
                if (LogicalTreeHelper.FindLogicalNode(this, ElementImage) is Path image)
                {
                    image.Data = _image;
                    image.Fill = _imageBrush;
                    image.Show();
                }
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                messageBox._showImage = true;
                messageBox._image = ResourceHelper.GetResource<Geometry>(ResourceToken.SuccessGeometry);
                messageBox._imageBrush = ResourceHelper.GetResource<Brush>(ResourceToken.SuccessBrush);
                messageBox.ShowDialog();
            });

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                SetImage(messageBox, MessageBoxImage.Information);
                messageBox.ShowDialog();
            });

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                SetImage(messageBox, MessageBoxImage.Warning);
                messageBox.ShowDialog();
            });

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                SetImage(messageBox, MessageBoxImage.Error);
                messageBox.ShowDialog();
            });

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
                SetButtonStatus(messageBox, MessageBoxButton.OK);
                messageBox._showImage = true;
                messageBox._image = ResourceHelper.GetResource<Geometry>(ResourceToken.FatalGeometry);
                messageBox._imageBrush = ResourceHelper.GetResource<Brush>(ResourceToken.PrimaryTextBrush);
                messageBox.ShowDialog();
            });

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, messageBoxText, caption, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
                SetButtonStatus(messageBox, MessageBoxButton.OKCancel);
                SetImage(messageBox, MessageBoxImage.Question);
                messageBox.ShowDialog();
            });

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(null, info.MessageBoxText, info.Caption, info.Button, MessageBoxImage.None, info.DefaultResult);
                SetButtonStatus(messageBox, info.Button);

                if (!string.IsNullOrEmpty(info.IconKey))
                {
                    messageBox._showImage = true;
                    messageBox._image = ResourceHelper.GetResource<Geometry>(info.IconKey);
                    messageBox._imageBrush = ResourceHelper.GetResource<Brush>(info.IconBrushKey);
                }

                messageBox.Style = info.Style;
                messageBox.ShowDialog();
            });

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = CreateMessageBox(owner, messageBoxText, caption, button, icon, defaultResult);
                SetButtonStatus(messageBox, button);
                SetImage(messageBox, icon);
                messageBox.ShowDialog();
            });

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

            var ownerWindow = owner ?? VisualHelper.GetActiveWindow();
            var ownerIsNull = ownerWindow is null;

            return new MessageBox
            {
                _message = messageBoxText,
                Owner = ownerWindow,
                WindowStartupLocation = ownerIsNull ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner,
                ShowTitle = true,
                Title = caption,
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
            messageBox._showImage = true;
            messageBox._image = ResourceHelper.GetResource<Geometry>(iconKey);
            messageBox._imageBrush = ResourceHelper.GetResource<Brush>(iconBrushKey);
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