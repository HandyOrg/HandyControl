using System;

namespace HandyControlDemo.Views
{
    public partial class GifImageDemoCtl : IDisposable
    {
        public GifImageDemoCtl()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            GifImageMain.Dispose();
        }
    }
}
