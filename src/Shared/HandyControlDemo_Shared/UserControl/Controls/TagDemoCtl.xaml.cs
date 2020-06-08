using System;
using HandyControl.Controls;
using HandyControlDemo.Properties.Langs;

namespace HandyControlDemo.UserControl
{
    public partial class TagDemoCtl
    {
        public TagDemoCtl()
        {
            InitializeComponent();
        }

        private void TagPanel_OnAddTagButtonClick(object sender, EventArgs e)
        {
            if (sender is TagPanel panel)
            {
                var tag = new Tag();
                LangProvider.SetLang(tag, ContentProperty, LangKeys.SubTitle);
                panel.Children.Add(tag);
            }
        }
    }
}
