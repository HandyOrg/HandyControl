using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementGrowPanel, Type = typeof(Panel))]
    internal class GrowlWindow : Window
    {
        private const string ElementGrowPanel = "PART_GrowPanel";

        public Panel GrowlPanel { get; set; }

        public GrowlWindow()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            GrowlPanel = GetTemplateChild(ElementGrowPanel) as Panel;
        }

        public void Init()
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Height = desktopWorkingArea.Height;
            Left = desktopWorkingArea.Right - Width;
            Top = 0;
        }
    }
}