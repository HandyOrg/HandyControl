using HandyControl.Tools.Interop;
using System;
using System.Diagnostics.CodeAnalysis;

namespace HandyControl.Data
{
    public sealed class Icon : IDisposable
    {
        private IntPtr _handle;

        ~Icon()
        {
            Dispose(false);
        }

        internal Icon(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException("InvalidGDIHandle", typeof(Icon).Name);
            }
            _handle = handle;
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void Dispose(bool disposing)
        {
            if (_handle != IntPtr.Zero)
            {
                UnsafeNativeMethods.DestroyIcon(_handle);
                _handle = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}