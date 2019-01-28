using System;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HandyControlDemo.Data;
using Microsoft.Win32;

namespace HandyControlDemo.ViewModel
{
    public class ChatBoxViewModel : ViewModelBase
    {
        private readonly string _id = Guid.NewGuid().ToString();

        public ChatBoxViewModel()
        {
            Messenger.Default.Register<ChatInfoModel>(this, MessageToken.SendChatString, ReceiveString);
            Messenger.Default.Register<ChatInfoModel>(this, MessageToken.SendChatImage, ReceiveImage);
            Messenger.Default.Register<ChatInfoModel>(this, MessageToken.SendChatAudio, ReceiveAudio);
        }

        private void ReceiveString(ChatInfoModel info)
        {
            if (!_id.Equals(info.SenderId)) return;
        }

        private void ReceiveImage(ChatInfoModel info)
        {
            if (!_id.Equals(info.SenderId)) return;
        }

        private void ReceiveAudio(ChatInfoModel info)
        {
            if (!_id.Equals(info.SenderId)) return;
        }

        private string _chatString;

        public string ChatString
        {
            get => _chatString;
            set => Set(ref _chatString, value);
        }

        public RelayCommand<KeyEventArgs> SendStringCmd => new Lazy<RelayCommand<KeyEventArgs>>(() =>
            new RelayCommand<KeyEventArgs>(SendString)).Value;

        private void SendString(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(ChatString)) return;
                Messenger.Default.Send(new ChatInfoModel
                {
                    Message = ChatString,
                    SenderId = _id
                }, MessageToken.SendChatString);
                ChatString = string.Empty;
            }
        }

        public RelayCommand OpenImageCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(OpenImage)).Value;

        private void OpenImage()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                if (File.Exists(fileName))
                {
                    Messenger.Default.Send(new ChatInfoModel
                    {
                        Message = fileName,
                        SenderId = _id
                    }, MessageToken.SendChatImage);
                }
            }
        }
    }
}