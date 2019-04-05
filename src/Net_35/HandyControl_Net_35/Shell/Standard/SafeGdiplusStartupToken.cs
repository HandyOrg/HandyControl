namespace Standard
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ConstrainedExecution;

    internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeGdiplusStartupToken() : base(true)
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            return (Standard.NativeMethods.GdiplusShutdown(base.handle) == Standard.Status.Ok);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static Standard.SafeGdiplusStartupToken Startup()
        {
            IntPtr ptr;
            Standard.StartupOutput output;
            Standard.SafeGdiplusStartupToken token = new Standard.SafeGdiplusStartupToken();
            if (Standard.NativeMethods.GdiplusStartup(out ptr, new Standard.StartupInput(), out output) == Standard.Status.Ok)
            {
                token.handle = ptr;
                return token;
            }
            token.Dispose();
            throw new Exception("Unable to initialize GDI+");
        }
    }
}

