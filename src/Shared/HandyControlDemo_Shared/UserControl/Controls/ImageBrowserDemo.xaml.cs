
using System;

namespace HandyControlDemo.UserControl;

public partial class ImageBrowserDemo : IDisposable
{
    public ImageBrowserDemo()
    {
        InitializeComponent();
    }

    public void Dispose()
    {
        ImageViewerDemo.Dispose();
    }
}
