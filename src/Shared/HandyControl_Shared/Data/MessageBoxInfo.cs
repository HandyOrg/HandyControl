using System.Windows;

namespace HandyControl.Data
{
    public class MessageBoxInfo
    {
        public string MessageBoxText { get; set; }

        public string Caption { get; set; }

        public MessageBoxButton Button { get; set; } = MessageBoxButton.OK;

        public string IconKey { get; set; }

        public string IconBrushKey { get; set; }

        public MessageBoxResult DefaultResult { get; set; } = MessageBoxResult.None;

        public Style Style { get; set; }
    }
}