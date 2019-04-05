namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct NONCLIENTMETRICS
    {
        public int cbSize;
        public int iBorderWidth;
        public int iScrollWidth;
        public int iScrollHeight;
        public int iCaptionWidth;
        public int iCaptionHeight;
        public Standard.LOGFONT lfCaptionFont;
        public int iSmCaptionWidth;
        public int iSmCaptionHeight;
        public Standard.LOGFONT lfSmCaptionFont;
        public int iMenuWidth;
        public int iMenuHeight;
        public Standard.LOGFONT lfMenuFont;
        public Standard.LOGFONT lfStatusFont;
        public Standard.LOGFONT lfMessageFont;
        public int iPaddedBorderWidth;
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static Standard.NONCLIENTMETRICS VistaMetricsStruct
        {
            get
            {
                return new Standard.NONCLIENTMETRICS { cbSize = Marshal.SizeOf(typeof(Standard.NONCLIENTMETRICS)) };
            }
        }
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static Standard.NONCLIENTMETRICS XPMetricsStruct
        {
            get
            {
                return new Standard.NONCLIENTMETRICS { cbSize = Marshal.SizeOf(typeof(Standard.NONCLIENTMETRICS)) - 4 };
            }
        }
    }
}

