using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HandyControl.Controls
{
    public class RibbonTabsPanel : Grid
    {
        protected override Geometry GetLayoutClip(Size layoutSlotSize) => null;
    }
}
