using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace HandyControl.Themes;

// ReSharper disable once PartialTypeWithSinglePart
public partial class Theme : Styles
{
    public Theme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}
