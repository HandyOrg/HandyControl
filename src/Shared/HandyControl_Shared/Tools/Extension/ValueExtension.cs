using System.Windows;

namespace HandyControl.Tools.Extension
{
    public static class ValueExtension
    {
        public static Thickness Add(this Thickness a, Thickness b) => new Thickness(a.Left + b.Left, a.Top + b.Top, a.Right + b.Right, a.Bottom + b.Bottom);
    }
}