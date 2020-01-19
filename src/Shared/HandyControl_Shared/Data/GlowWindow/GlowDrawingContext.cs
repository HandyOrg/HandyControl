using System;
using HandyControl.Tools.Interop;

namespace HandyControl.Data
{
    internal class GlowDrawingContext : DisposableObject
    {
        private readonly GlowBitmap _windowBitmap;
        
        internal InteropValues.BLENDFUNCTION Blend;

        internal GlowDrawingContext(int width, int height)
        {
            ScreenDC = InteropMethods.GetDC(IntPtr.Zero);
            if (ScreenDC == IntPtr.Zero) return;
            WindowDC = InteropMethods.CreateCompatibleDC(ScreenDC);
            if (WindowDC == IntPtr.Zero) return;
            BackgroundDC = InteropMethods.CreateCompatibleDC(ScreenDC);
            if (BackgroundDC == IntPtr.Zero) return;
            Blend.BlendOp = 0;
            Blend.BlendFlags = 0;
            Blend.SourceConstantAlpha = 255;
            Blend.AlphaFormat = 1;
            _windowBitmap = new GlowBitmap(ScreenDC, width, height);
            InteropMethods.SelectObject(WindowDC, _windowBitmap.Handle);
        }

        internal bool IsInitialized => 
            ScreenDC != IntPtr.Zero && WindowDC != IntPtr.Zero && 
            BackgroundDC != IntPtr.Zero && _windowBitmap != null;

        internal IntPtr ScreenDC { get; }

        internal IntPtr WindowDC { get; }

        internal IntPtr BackgroundDC { get; }

        internal int Width => _windowBitmap.Width;

        internal int Height => _windowBitmap.Height;

        protected override void DisposeManagedResources() => _windowBitmap.Dispose();

        protected override void DisposeNativeResources()
        {
            if (ScreenDC != IntPtr.Zero) InteropMethods.ReleaseDC(IntPtr.Zero, ScreenDC);
            if (WindowDC != IntPtr.Zero) InteropMethods.DeleteDC(WindowDC);
            if (BackgroundDC != IntPtr.Zero) InteropMethods.DeleteDC(BackgroundDC);
        }
    }
}
