using System;

namespace HandyControlDemo.UserControl;

public partial class GifImageDemo : IDisposable
{
    public GifImageDemo()
    {
        InitializeComponent();
    }

    public void Dispose()
    {
        GifImageMain.Dispose();
    }
}
