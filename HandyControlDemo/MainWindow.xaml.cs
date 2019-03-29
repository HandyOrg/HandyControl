using System;
using System.ComponentModel;
using HandyControl.Controls;
using HandyControlDemo.Data;

namespace HandyControlDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            Growl.SetGrowlParent(GrowlPanel, true);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            Growl.SetGrowlParent(GrowlPanel, false);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (GlobalData.NotifyIconIsShow)
            {
                MessageBox.Info(Properties.Langs.Lang.AppClosingTip, Properties.Langs.Lang.Tip);
                Hide();
                e.Cancel = true;
            }
            else
            {
                base.OnClosing(e);
            }
        }
    }
}