namespace Standard
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct HRESULT
    {
        [FieldOffset(0)]
        private readonly uint _value;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT COR_E_OBJECTDISPOSED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT DESTS_E_NORECDOCS;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT DESTS_E_NOTALLCLEARED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_ABORT;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_ACCESSDENIED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_FAIL;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_INVALIDARG;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_NOINTERFACE;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_NOTIMPL;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_OUTOFMEMORY;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_PENDING;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_POINTER;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT E_UNEXPECTED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT REGDB_E_CLASSNOTREG;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT S_FALSE;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT S_OK;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT STG_E_INVALIDFUNCTION;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT WC_E_GREATERTHAN;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.HRESULT WC_E_SYNTAX;

        static HRESULT()
        {
            S_OK = new Standard.HRESULT(0);
            S_FALSE = new Standard.HRESULT(1);
            E_PENDING = new Standard.HRESULT(0x8000000a);
            E_NOTIMPL = new Standard.HRESULT(0x80004001);
            E_NOINTERFACE = new Standard.HRESULT(0x80004002);
            E_POINTER = new Standard.HRESULT(0x80004003);
            E_ABORT = new Standard.HRESULT(0x80004004);
            E_FAIL = new Standard.HRESULT(0x80004005);
            E_UNEXPECTED = new Standard.HRESULT(0x8000ffff);
            STG_E_INVALIDFUNCTION = new Standard.HRESULT(0x80030001);
            REGDB_E_CLASSNOTREG = new Standard.HRESULT(0x80040154);
            DESTS_E_NO_MATCHING_ASSOC_HANDLER = new Standard.HRESULT(0x80040f03);
            DESTS_E_NORECDOCS = new Standard.HRESULT(0x80040f04);
            DESTS_E_NOTALLCLEARED = new Standard.HRESULT(0x80040f05);
            E_ACCESSDENIED = new Standard.HRESULT(0x80070005);
            E_OUTOFMEMORY = new Standard.HRESULT(0x8007000e);
            E_INVALIDARG = new Standard.HRESULT(0x80070057);
            INTSAFE_E_ARITHMETIC_OVERFLOW = new Standard.HRESULT(0x80070216);
            COR_E_OBJECTDISPOSED = new Standard.HRESULT(0x80131622);
            WC_E_GREATERTHAN = new Standard.HRESULT(0xc00cee23);
            WC_E_SYNTAX = new Standard.HRESULT(0xc00cee2d);
        }

        public HRESULT(uint i)
        {
            this._value = i;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return (((Standard.HRESULT) obj)._value == this._value);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public static int GetCode(int error)
        {
            return (error & 0xffff);
        }

        public static Standard.Facility GetFacility(int errorCode)
        {
            return (((Standard.Facility) (errorCode >> 0x10)) & ((Standard.Facility) 0x1fff));
        }

        public override int GetHashCode()
        {
            return this._value.GetHashCode();
        }

        public static Standard.HRESULT Make(bool severe, Standard.Facility facility, int code)
        {
            return new Standard.HRESULT((uint) (((severe ? -2147483648 : 0) | (((int) facility) << 0x10)) | code));
        }

        public static bool operator ==(Standard.HRESULT hrLeft, Standard.HRESULT hrRight)
        {
            return (hrLeft._value == hrRight._value);
        }

        public static bool operator !=(Standard.HRESULT hrLeft, Standard.HRESULT hrRight)
        {
            return !(hrLeft == hrRight);
        }

        public void ThrowIfFailed()
        {
            this.ThrowIfFailed(null);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification="Only recreating Exceptions that were already raised."), SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void ThrowIfFailed(string message)
        {
            if (this.Failed)
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = this.ToString();
                }
                Exception exceptionForHR = Marshal.GetExceptionForHR((int) this._value, new IntPtr(-1));
                if (exceptionForHR.GetType() == typeof(COMException))
                {
                    if (this.Facility == Standard.Facility.Win32)
                    {
                        exceptionForHR = new Win32Exception(this.Code, message);
                    }
                    else
                    {
                        exceptionForHR = new COMException(message, (int) this._value);
                    }
                }
                else
                {
                    ConstructorInfo constructor = exceptionForHR.GetType().GetConstructor(new Type[] { typeof(string) });
                    if (null != constructor)
                    {
                        exceptionForHR = constructor.Invoke(new object[] { message }) as Exception;
                    }
                }
                throw exceptionForHR;
            }
        }

        public static void ThrowLastError()
        {
            ((Standard.HRESULT) Standard.Win32Error.GetLastError()).ThrowIfFailed();
        }

        public override string ToString()
        {
            foreach (FieldInfo info in typeof(Standard.HRESULT).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (info.FieldType == typeof(Standard.HRESULT))
                {
                    Standard.HRESULT hresult = (Standard.HRESULT) info.GetValue(null);
                    if (hresult == this)
                    {
                        return info.Name;
                    }
                }
            }
            if (this.Facility == Standard.Facility.Win32)
            {
                foreach (FieldInfo info2 in typeof(Standard.Win32Error).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if (info2.FieldType == typeof(Standard.Win32Error))
                    {
                        Standard.Win32Error error = (Standard.Win32Error) info2.GetValue(null);
                        if (((Standard.HRESULT) error) == this)
                        {
                            return ("HRESULT_FROM_WIN32(" + info2.Name + ")");
                        }
                    }
                }
            }
            return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", new object[] { this._value });
        }

        public int Code
        {
            get
            {
                return GetCode((int) this._value);
            }
        }

        public Standard.Facility Facility
        {
            get
            {
                return GetFacility((int) this._value);
            }
        }

        public bool Failed
        {
            get
            {
                return (this._value < 0);
            }
        }

        public bool Succeeded
        {
            get
            {
                return (this._value >= 0);
            }
        }
    }
}

