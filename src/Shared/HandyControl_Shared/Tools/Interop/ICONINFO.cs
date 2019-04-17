using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace HandyControl.Tools.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class ICONINFO
    {
        public bool fIcon = false;
        public int xHotspot = 0;
        public int yHotspot = 0;
        public BitmapHandle hbmMask = null;
        public BitmapHandle hbmColor = null;
    }
}