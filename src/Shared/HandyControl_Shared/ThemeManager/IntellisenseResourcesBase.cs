using System;
using System.ComponentModel;
using System.Windows;

namespace HandyControl.ThemeManager
{
    public abstract class IntellisenseResourcesBase : ResourceDictionary, ISupportInitialize
    {
        protected IntellisenseResourcesBase()
        {
        }

        public new Uri Source
        {
            get => base.Source;
            set
            {
                if (DesignMode.DesignModeEnabled)
                {
                    base.Source = value;
                }
            }
        }

        public new void EndInit()
        {
            Clear();
            MergedDictionaries.Clear();
            base.EndInit();
        }

        void ISupportInitialize.EndInit()
        {
            EndInit();
        }
    }
}
