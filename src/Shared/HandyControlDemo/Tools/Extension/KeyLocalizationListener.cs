using System.ComponentModel;

namespace HandyControlDemo.Tools.Extension
{
    /// <summary>
    /// Listener for cultural change when localized by key
    /// </summary>
    public class KeyLocalizationListener : BaseLocalizationListener, INotifyPropertyChanged
    {
        public KeyLocalizationListener(string key, object[] args)
        {
            Key = key;
            Args = args;
        }

        private string Key { get; }

        private object[] Args { get; }

        public object Value
        {
            get
            {
                var value = LocalizationManager.Instance.Localize(Key);
                if (value is string && Args != null)
                {
                    value = string.Format((string)value, Args);
                }

                return value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnCultureChanged()
        {
            // Notify string change binding
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
