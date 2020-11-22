using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.Tools;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HandyControlDemo.ViewModels
{
    public class MessageSentEvent : PubSubEvent<ChatInfoModel>
    {
    }

    public class ChatBoxViewModel : BindableBase
    {
        IEventAggregator _ea;

        private static readonly string AudioCachePath = $"{AppDomain.CurrentDomain.BaseDirectory}Cache";

        private readonly string _id = Guid.NewGuid().ToString();

        private readonly Stopwatch _stopwatch = new Lazy<Stopwatch>(() => new Stopwatch()).Value;

        public ChatBoxViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<MessageSentEvent>().Subscribe(ReceiveMessage);
        }

        private void ReceiveMessage(ChatInfoModel info)
        {
            if (_id.Equals(info.SenderId))
            {
                return;
            }

            info.Role = ChatRoleType.Receiver;
            ChatInfos.Add(info);
        }

        private string _chatString;

        public string ChatString
        {
            get => _chatString;
            set => SetProperty(ref _chatString, value);
        }

        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; } = new ObservableCollection<ChatInfoModel>();

        private DelegateCommand<KeyEventArgs> _SendStringCmd;
        public DelegateCommand<KeyEventArgs> SendStringCmd =>
            _SendStringCmd ?? (_SendStringCmd = new DelegateCommand<KeyEventArgs>(SendString));

        private void SendString(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(ChatString))
                {
                    return;
                }

                var info = new ChatInfoModel
                {
                    Message = ChatString,
                    SenderId = _id,
                    Type = ChatMessageType.String,
                    Role = ChatRoleType.Sender
                };
                ChatInfos.Add(info);
                _ea.GetEvent<MessageSentEvent>().Publish(info);
                ChatString = string.Empty;
            }
        }

        private DelegateCommand<RoutedEventArgs> _ReadMessageCmd;
        public DelegateCommand<RoutedEventArgs> ReadMessageCmd =>
            _ReadMessageCmd ?? (_ReadMessageCmd = new DelegateCommand<RoutedEventArgs>(ReadMessage));

        private void ReadMessage(RoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.Tag is ChatInfoModel info)
            {
                if (info.Type == ChatMessageType.Image)
                {
                    new ImageBrowser(new Uri(info.Enclosure.ToString()))
                    {
                        Owner = WindowHelper.GetActiveWindow()
                    }.Show();
                }
                else if (info.Type == ChatMessageType.Audio)
                {
                    var player = new SoundPlayer(info.Enclosure.ToString());
                    player.PlaySync();
                }
            }
        }

        private DelegateCommand _StartRecordCmd;
        public DelegateCommand StartRecordCmd =>
            _StartRecordCmd ?? (_StartRecordCmd = new DelegateCommand(StartRecord));

        private void StartRecord()
        {
            Win32Helper.MciSendString("set wave bitpersample 8", "", 0, 0);
            Win32Helper.MciSendString("set wave samplespersec 20000", "", 0, 0);
            Win32Helper.MciSendString("set wave channels 2", "", 0, 0);
            Win32Helper.MciSendString("set wave format tag pcm", "", 0, 0);
            Win32Helper.MciSendString("open new type WAVEAudio alias movie", "", 0, 0);
            Win32Helper.MciSendString("record movie", "", 0, 0);

            _stopwatch.Reset();
            _stopwatch.Start();
        }

        private DelegateCommand _StopRecordCmd;
        public DelegateCommand StopRecordCmd =>
            _StopRecordCmd ?? (_StopRecordCmd = new DelegateCommand(StopRecord));

        private void StopRecord()
        {
            if (!Directory.Exists(AudioCachePath))
            {
                try
                {
                    Directory.CreateDirectory(AudioCachePath);
                }
                catch (Exception e)
                {
                    Growl.Error(e.Message);
                    return;
                }
            }

            var cachePath = $"{AudioCachePath}\\{Guid.NewGuid().ToString()}";
            var cachePathWithQuotes = $"\"{cachePath}\"";
            Win32Helper.MciSendString("stop movie", "", 0, 0);
            Win32Helper.MciSendString($"save movie {cachePathWithQuotes}", "", 0, 0);
            Win32Helper.MciSendString("close movie", "", 0, 0);

            _stopwatch.Stop();

            var info = new ChatInfoModel
            {
                Message = $"{_stopwatch.Elapsed.Seconds.ToString()} {Properties.Langs.Lang.Second}",
                SenderId = _id,
                Type = ChatMessageType.Audio,
                Role = ChatRoleType.Sender,
                Enclosure = cachePath
            };
            ChatInfos.Add(info);
            _ea.GetEvent<MessageSentEvent>().Publish(info);
        }

        private DelegateCommand _OpenImageCmd;
        public DelegateCommand OpenImageCmd =>
            _OpenImageCmd ?? (_OpenImageCmd = new DelegateCommand(OpenImage));

        private void OpenImage()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                if (File.Exists(fileName))
                {
                    var info = new ChatInfoModel
                    {
                        Message = BitmapFrame.Create(new Uri(fileName)),
                        SenderId = _id,
                        Type = ChatMessageType.Image,
                        Role = ChatRoleType.Sender,
                        Enclosure = fileName
                    };
                    ChatInfos.Add(info);
                    _ea.GetEvent<MessageSentEvent>().Publish(info);
                }
            }
        }
    }
}