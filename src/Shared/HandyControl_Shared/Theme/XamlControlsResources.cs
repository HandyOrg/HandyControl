using System;
using System.Windows;

namespace HandyControl.Controls
{
    /// <summary>
    /// Default styles for controls.
    /// </summary>
    public class XamlControlsResources : ResourceDictionary
    {
        /// <summary>
        /// Initializes a new instance of the XamlControlsResources class.
        /// </summary>
        public XamlControlsResources()
        {
            MergedDictionaries.Add(ControlsResources);
        }

        internal static ResourceDictionary ControlsResources
        {
            get
            {
                if (_controlsResources == null)
                {
                    _controlsResources = new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") };
                }
                return _controlsResources;
            }
        }


        private static ResourceDictionary _controlsResources;
    }
}
