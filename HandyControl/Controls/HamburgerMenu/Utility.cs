using System.ComponentModel;
using System.Windows;

namespace HandyControl.Controls
{
    public class Utility
    {
        /// <summary>
        /// 刷新样式
        /// </summary>
        /// <param name="control"></param>
        public static void Refresh(FrameworkElement control)
        {
            if (control == null)
            {
                return;
            }
            if (!DesignerProperties.GetIsInDesignMode(control))
            {
                if (control.IsLoaded)
                {
                    SetColor(control);
                }
                else
                {
                    control.Loaded += delegate { SetColor(control); };
                }
            }
        }

        static void SetColor(FrameworkElement control)
        {
            var mw = Window.GetWindow(control);
            if (mw != null)
                if (control is HamburgerTabControl) { (control as HamburgerTabControl).BorderBrush = mw.BorderBrush; }
        }
    }
}
