using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControlDemo.Data;
using HandyControlDemo.Tools;
using Microsoft.Win32;

namespace HandyControlDemo.ViewModel
{
    public class ChatBoxViewModel : ViewModelBase
    {
        private static readonly string AudioCachePath = $"{AppDomain.CurrentDomain.BaseDirectory}Cache";

        private readonly string _id = Guid.NewGuid().ToString();

        private readonly Stopwatch _stopwatch = new Lazy<Stopwatch>(() => new Stopwatch()).Value;

        public ChatBoxViewModel()
        {
            Messenger.Default.Register<ChatInfoModel>(this, MessageToken.SendChatMessage, ReceiveMessage);
        }

        private void ReceiveMessage(ChatInfoModel info)
        {
            if (_id.Equals(info.SenderId)) return;
            info.Role = ChatRoleType.Receiver;
            ChatInfos.Add(info);
        }

        private string _chatString;

        public string ChatString
        {
            get => _chatString;
#if NET40
            set => Set(nameof(ChatString), ref _chatString, value);
#else
            set => Set(ref _chatString, value);
#endif
        }

        public ObservableCollection<ChatInfoModel> ChatInfos { get; set; } = new ObservableCollection<ChatInfoModel>();

        public RelayCommand<KeyEventArgs> SendStringCmd => new Lazy<RelayCommand<KeyEventArgs>>(() =>
            new RelayCommand<KeyEventArgs>(SendString)).Value;

        private void SendString(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(ChatString)) return;
                var info = new ChatInfoModel
                {
                    Message = ChatString,
                    SenderId = _id,
                    Type = ChatMessageType.String,
                    Role = ChatRoleType.Sender
                };
                ChatInfos.Add(info);
                Messenger.Default.Send(info, MessageToken.SendChatMessage);
                ChatString = string.Empty;
            }
        }

        public RelayCommand<RoutedEventArgs> ReadMessageCmd => new Lazy<RelayCommand<RoutedEventArgs>>(() =>
            new RelayCommand<RoutedEventArgs>(ReadMessage)).Value;

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

        public RelayCommand StartRecordCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(StartRecord)).Value;

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

        public RelayCommand StopRecordCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(StopRecord)).Value;

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
            Messenger.Default.Send(info, MessageToken.SendChatMessage);
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
                    var info = new ChatInfoModel
                    {
                        Message = BitmapFrame.Create(new Uri(fileName)),
                        SenderId = _id,
                        Type = ChatMessageType.Image,
                        Role = ChatRoleType.Sender,
                        Enclosure = fileName
                    };
                    ChatInfos.Add(info);
                    Messenger.Default.Send(info, MessageToken.SendChatMessage);
                }
            }
        }
    }
}