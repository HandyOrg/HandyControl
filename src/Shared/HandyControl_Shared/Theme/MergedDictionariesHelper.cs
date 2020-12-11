using System.Collections.ObjectModel;
using System.Windows;

namespace HandyControl.Controls
{
    internal static class MergedDictionariesHelper
    {
        public static void RemoveIfNotNull(this Collection<ResourceDictionary> mergedDictionaries, ResourceDictionary item)
        {
            if (item != null)
            {
                mergedDictionaries.Remove(item);
            }
        }

        public static void InsertOrReplace(this Collection<ResourceDictionary> mergedDictionaries, int index, ResourceDictionary item)
        {
            if (mergedDictionaries.Count > index)
            {
                mergedDictionaries[index] = item;
            }
            else
            {
                mergedDictionaries.Insert(index, item);
            }
        }

        public static void RemoveAll<T>(this Collection<ResourceDictionary> mergedDictionaries) where T : ResourceDictionary
        {
            for (int i = mergedDictionaries.Count - 1; i >= 0; i--)
            {
                if (mergedDictionaries[i] is T)
                {
                    mergedDictionaries.RemoveAt(i);
                }
            }
        }

        public static void InsertIfNotExists(this Collection<ResourceDictionary> mergedDictionaries, int index, ResourceDictionary item)
        {
            if (!mergedDictionaries.Contains(item))
            {
                mergedDictionaries.Insert(index, item);
            }
        }
    }
}
