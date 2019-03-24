using System;
using HandyControl.Controls;

namespace HandyControlDemo.Window
{
    public partial class GrowlDemoWindow
    {
        public GrowlDemoWindow()
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
    }
}
