using System.Collections.Generic;
using System.Windows;

namespace HandyControl.ThemeManager
{
    public class ResourceDictionaryEx : ResourceDictionary
    {
        private ResourceDictionary _mergedThemeDictionary;

        /// <summary>
        /// Gets a collection of merged resource dictionaries that are specifically keyed
        /// and composed to address theme scenarios, for example supplying theme values for
        /// "HighContrast".
        /// </summary>
        /// <returns>
        /// A dictionary of ResourceDictionary theme dictionaries. Each must be keyed with
        /// **x:Key**.
        /// </returns>
        public Dictionary<object, ResourceDictionary> ThemeDictionaries { get; } = new Dictionary<object, ResourceDictionary>();

        public ResourceDictionary MergedAppThemeDictionary { get; set; }

        internal void Update(string themeKey)
        {
            if (ThemeDictionaries.TryGetValue(themeKey, out ResourceDictionary themeDictionary))
            {
                if (_mergedThemeDictionary != null)
                {
                    if (_mergedThemeDictionary == themeDictionary)
                    {
                        return;
                    }
                    else
                    {
                        int targetIndex = MergedDictionaries.IndexOf(_mergedThemeDictionary);
                        MergedDictionaries[targetIndex] = themeDictionary;
                        _mergedThemeDictionary = themeDictionary;
                    }
                }
                else
                {
                    int targetIndex;

                    if (MergedAppThemeDictionary != null)
                    {
                        targetIndex = MergedDictionaries.IndexOf(MergedAppThemeDictionary) + 1;
                    }
                    else
                    {
                        targetIndex = 0;
                    }

                    MergedDictionaries.Insert(targetIndex, themeDictionary);
                    _mergedThemeDictionary = themeDictionary;
                }
            }
            else
            {
                if (_mergedThemeDictionary != null)
                {
                    MergedDictionaries.Remove(_mergedThemeDictionary);
                    _mergedThemeDictionary = null;
                }
            }
        }
    }
}
