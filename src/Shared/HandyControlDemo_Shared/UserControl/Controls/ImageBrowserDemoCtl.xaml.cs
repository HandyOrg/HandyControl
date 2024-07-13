
using System;

namespace HandyControlDemo.UserControl;

public partial class ImageBrowserDemoCtl : IDisposable
{
    public ImageBrowserDemoCtl()
    {
        InitializeComponent();
    }

    public void Dispose()
    {
        ImageViewerDemo.Dispose();
    }
}
