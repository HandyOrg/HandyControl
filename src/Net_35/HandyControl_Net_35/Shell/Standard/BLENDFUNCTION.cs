namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct BLENDFUNCTION
    {
        public Standard.AC BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public Standard.AC AlphaFormat;
    }
}

