namespace Microsoft.Windows.Shell
{
    using System.Windows;

    public class ThumbButtonInfoCollection : FreezableCollection<ThumbButtonInfo>
    {
        private static ThumbButtonInfoCollection s_empty;

        protected override Freezable CreateInstanceCore()
        {
            return new ThumbButtonInfoCollection();
        }

        internal static ThumbButtonInfoCollection Empty
        {
            get
            {
                if (s_empty == null)
                {
                    ThumbButtonInfoCollection infos = new ThumbButtonInfoCollection();
                    infos.Freeze();
                    s_empty = infos;
                }
                return s_empty;
            }
        }
    }
}

