using System.Windows;

namespace HandyControl.ThemeManager
{
    public static class ResourceDictionaryHelper
    {
        public static void SealValues(this ResourceDictionary dictionary)
        {
            foreach (var md in dictionary.MergedDictionaries)
            {
                SealValues(md);
            }

            foreach (var value in dictionary.Values)
            {
                if (value is Freezable freezable)
                {
                    if (!freezable.CanFreeze)
                    {
                        var enumerator = freezable.GetLocalValueEnumerator();
                        while (enumerator.MoveNext())
                        {
                            var property = enumerator.Current.Property;
                            if (DependencyPropertyHelper.GetValueSource(freezable, property).IsExpression)
                            {
                                freezable.SetValue(property, freezable.GetValue(property));
                            }
                        }
                    }

                    if (!freezable.IsFrozen)
                    {
                        freezable.Freeze();
                    }
                }
                else if (value is Style style)
                {
                    if (!style.IsSealed)
                    {
                        style.Seal();
                    }
                }
            }

            if (dictionary is ResourceDictionaryEx rdEx)
            {
                foreach (var td in rdEx.ThemeDictionaries.Values)
                {
                    SealValues(td);
                }
            }
        }
    }
}
