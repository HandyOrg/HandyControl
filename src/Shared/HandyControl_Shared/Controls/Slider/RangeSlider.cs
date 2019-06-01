using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace HandyControl.Controls
{
    [DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    [TemplatePart(Name = "PART_Track", Type = typeof(Track))]
    [TemplatePart(Name = "PART_SelectionRange", Type = typeof(FrameworkElement))]
    public class RangeSlider : RangeBase
    {
        

    }
}