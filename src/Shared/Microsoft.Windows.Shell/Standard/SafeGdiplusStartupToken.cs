using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Standard;

internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeGdiplusStartupToken() : base(true)
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
        Status status = NativeMethods.GdiplusShutdown(this.handle);
        return status == Status.Ok;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
    [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
    public static SafeGdiplusStartupToken Startup()
    {
        SafeGdiplusStartupToken safeGdiplusStartupToken = new SafeGdiplusStartupToken();
        IntPtr handle;
        StartupOutput startupOutput;
        if (NativeMethods.GdiplusStartup(out handle, new StartupInput(), out startupOutput) == Status.Ok)
        {
            safeGdiplusStartupToken.handle = handle;
            return safeGdiplusStartupToken;
        }
        safeGdiplusStartupToken.Dispose();
        throw new Exception("Unable to initialize GDI+");
    }
}
