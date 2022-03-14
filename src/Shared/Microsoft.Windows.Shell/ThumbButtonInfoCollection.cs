using System;
using System.Windows;

namespace Microsoft.Windows.Shell;

public class ThumbButtonInfoCollection : FreezableCollection<ThumbButtonInfo>
{
    protected override Freezable CreateInstanceCore()
    {
        return new ThumbButtonInfoCollection();
    }

    internal static ThumbButtonInfoCollection Empty
    {
        get
        {
            if (ThumbButtonInfoCollection.s_empty == null)
            {
                ThumbButtonInfoCollection thumbButtonInfoCollection = new ThumbButtonInfoCollection();
                thumbButtonInfoCollection.Freeze();
                ThumbButtonInfoCollection.s_empty = thumbButtonInfoCollection;
            }
            return ThumbButtonInfoCollection.s_empty;
        }
    }

    private static ThumbButtonInfoCollection s_empty;
}
