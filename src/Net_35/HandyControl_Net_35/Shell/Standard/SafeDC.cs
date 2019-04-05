namespace Standard
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;

    internal sealed class SafeDC : SafeHandleZeroOrMinusOneIsInvalid
    {
        private bool _created;
        private IntPtr? _hwnd;

        private SafeDC() : base(true)
        {
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Standard.SafeDC CreateCompatibleDC(Standard.SafeDC hdc)
        {
            Standard.SafeDC edc = null;
            try
            {
                IntPtr zero = IntPtr.Zero;
                if (hdc != null)
                {
                    zero = hdc.handle;
                }
                edc = NativeMethods.CreateCompatibleDC(zero);
                if (edc == null)
                {
                    Standard.HRESULT.ThrowLastError();
                }
            }
            finally
            {
                if (edc != null)
                {
                    edc._created = true;
                }
            }
            if (edc.IsInvalid)
            {
                edc.Dispose();
                throw new SystemException("Unable to create a device context from the specified device information.");
            }
            return edc;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static Standard.SafeDC CreateDC(string deviceName)
        {
            Standard.SafeDC edc = null;
            try
            {
                edc = NativeMethods.CreateDC(deviceName, null, IntPtr.Zero, IntPtr.Zero);
            }
            finally
            {
                if (edc != null)
                {
                    edc._created = true;
                }
            }
            if (edc.IsInvalid)
            {
                edc.Dispose();
                throw new SystemException("Unable to create a device context from the specified device information.");
            }
            return edc;
        }

        public static Standard.SafeDC GetDC(IntPtr hwnd)
        {
            Standard.SafeDC dC = null;
            try
            {
                dC = NativeMethods.GetDC(hwnd);
            }
            finally
            {
                if (dC != null)
                {
                    dC.Hwnd = hwnd;
                }
            }
            if (dC.IsInvalid)
            {
                Standard.HRESULT.E_FAIL.ThrowIfFailed();
            }
            return dC;
        }

        public static Standard.SafeDC GetDesktop()
        {
            return GetDC(IntPtr.Zero);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            if (this._created)
            {
                return NativeMethods.DeleteDC(base.handle);
            }
            if (this._hwnd.HasValue && (this._hwnd.Value != IntPtr.Zero))
            {
                return (NativeMethods.ReleaseDC(this._hwnd.Value, base.handle) == 1);
            }
            return true;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Standard.SafeDC WrapDC(IntPtr hdc)
        {
            return new Standard.SafeDC { handle = hdc, _created = false, _hwnd = new IntPtr?(IntPtr.Zero) };
        }

        public IntPtr Hwnd
        {
            set
            {
                this._hwnd = new IntPtr?(value);
            }
        }

        private static class NativeMethods
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
            public static extern Standard.SafeDC CreateCompatibleDC(IntPtr hdc);
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll", CharSet=CharSet.Unicode)]
            public static extern Standard.SafeDC CreateDC([MarshalAs(UnmanagedType.LPWStr)] string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);
            [return: MarshalAs(UnmanagedType.Bool)]
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hdc);
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
            public static extern Standard.SafeDC GetDC(IntPtr hwnd);
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), DllImport("user32.dll")]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        }
    }
}

