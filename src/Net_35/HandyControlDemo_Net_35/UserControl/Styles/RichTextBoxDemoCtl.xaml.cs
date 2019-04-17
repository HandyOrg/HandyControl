using HandyControlDemo.Tools.Converter;

namespace HandyControlDemo.UserControl
{
    public partial class RichTextBoxDemoCtl
    {
        public RichTextBoxDemoCtl()
        {
            InitializeComponent();

            var converter = new StringRepeatConverter();
            converter.Convert(Properties.Langs.Lang.Title, null, 20.ToString(), null);
            RunTitle.Text = converter.Convert(Properties.Langs.Lang.Title, null, 20, null)?.ToString();
            RunText.Text = converter.Convert(Properties.Langs.Lang.Text, null, 1000, null)?.ToString();
        }
    }
}
