using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Standard;

internal sealed class SafeDC : SafeHandleZeroOrMinusOneIsInvalid
{
    public IntPtr Hwnd
    {
        set
        {
            this._hwnd = new IntPtr?(value);
        }
    }

    private SafeDC() : base(true)
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
        if (this._created)
        {
            return SafeDC.NativeMethods.DeleteDC(this.handle);
        }
        return this._hwnd == null || this._hwnd.Value == IntPtr.Zero || SafeDC.NativeMethods.ReleaseDC(this._hwnd.Value, this.handle) == 1;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
    public static SafeDC CreateDC(string deviceName)
    {
        SafeDC safeDC = null;
        try
        {
            safeDC = SafeDC.NativeMethods.CreateDC(deviceName, null, IntPtr.Zero, IntPtr.Zero);
        }
        finally
        {
            if (safeDC != null)
            {
                safeDC._created = true;
            }
        }
        if (safeDC.IsInvalid)
        {
            safeDC.Dispose();
            throw new SystemException("Unable to create a device context from the specified device information.");
        }
        return safeDC;
    }

    [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static SafeDC CreateCompatibleDC(SafeDC hdc)
    {
        SafeDC safeDC = null;
        try
        {
            IntPtr hdc2 = IntPtr.Zero;
            if (hdc != null)
            {
                hdc2 = hdc.handle;
            }
            safeDC = SafeDC.NativeMethods.CreateCompatibleDC(hdc2);
            if (safeDC == null)
            {
                HRESULT.ThrowLastError();
            }
        }
        finally
        {
            if (safeDC != null)
            {
                safeDC._created = true;
            }
        }
        if (safeDC.IsInvalid)
        {
            safeDC.Dispose();
            throw new SystemException("Unable to create a device context from the specified device information.");
        }
        return safeDC;
    }

    public static SafeDC GetDC(IntPtr hwnd)
    {
        SafeDC safeDC = null;
        try
        {
            safeDC = SafeDC.NativeMethods.GetDC(hwnd);
        }
        finally
        {
            if (safeDC != null)
            {
                safeDC.Hwnd = hwnd;
            }
        }
        if (safeDC.IsInvalid)
        {
            HRESULT.E_FAIL.ThrowIfFailed();
        }
        return safeDC;
    }

    public static SafeDC GetDesktop()
    {
        return SafeDC.GetDC(IntPtr.Zero);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
    public static SafeDC WrapDC(IntPtr hdc)
    {
        return new SafeDC
        {
            handle = hdc,
            _created = false,
            _hwnd = new IntPtr?(IntPtr.Zero)
        };
    }

    private IntPtr? _hwnd;

    private bool _created;

    private static class NativeMethods
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("user32.dll")]
        public static extern SafeDC GetDC(IntPtr hwnd);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern SafeDC CreateDC([MarshalAs(UnmanagedType.LPWStr)] string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeDC CreateCompatibleDC(IntPtr hdc);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);
    }
}
