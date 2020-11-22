using HandyControl.Tools;
using System.Windows.Controls;

namespace HandyControlDemo.Views
{
    public partial class ChatBox
    {
        private ScrollViewer _scrollViewer;

        public ChatBox()
        {
            InitializeComponent();
            ListBoxChat.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
        }

        private void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            _scrollViewer ??= VisualHelper.GetChild<ScrollViewer>(ListBoxChat);
            _scrollViewer?.ScrollToBottom();
        }
    }
}
