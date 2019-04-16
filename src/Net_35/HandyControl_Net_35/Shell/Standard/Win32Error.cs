namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct Win32Error
    {
        [FieldOffset(0)]
        private readonly int _value;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_ACCESS_DENIED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_BAD_DEVICE;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_CANCELLED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_CLASS_ALREADY_EXISTS;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_FILE_NOT_FOUND;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_INSUFFICIENT_BUFFER;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_INVALID_DATATYPE;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_INVALID_FUNCTION;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_INVALID_HANDLE;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_INVALID_PARAMETER;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_KEY_DELETED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_NESTING_NOT_ALLOWED;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_NO_MATCH;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_NO_MORE_FILES;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_NOT_FOUND;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_OUTOFMEMORY;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_PATH_NOT_FOUND;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_SHARING_VIOLATION;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_SUCCESS;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static readonly Standard.Win32Error ERROR_TOO_MANY_OPEN_FILES;

        static Win32Error()
        {
            ERROR_SUCCESS = new Standard.Win32Error(0);
            ERROR_INVALID_FUNCTION = new Standard.Win32Error(1);
            ERROR_FILE_NOT_FOUND = new Standard.Win32Error(2);
            ERROR_PATH_NOT_FOUND = new Standard.Win32Error(3);
            ERROR_TOO_MANY_OPEN_FILES = new Standard.Win32Error(4);
            ERROR_ACCESS_DENIED = new Standard.Win32Error(5);
            ERROR_INVALID_HANDLE = new Standard.Win32Error(6);
            ERROR_OUTOFMEMORY = new Standard.Win32Error(14);
            ERROR_NO_MORE_FILES = new Standard.Win32Error(0x12);
            ERROR_SHARING_VIOLATION = new Standard.Win32Error(0x20);
            ERROR_INVALID_PARAMETER = new Standard.Win32Error(0x57);
            ERROR_INSUFFICIENT_BUFFER = new Standard.Win32Error(0x7a);
            ERROR_NESTING_NOT_ALLOWED = new Standard.Win32Error(0xd7);
            ERROR_KEY_DELETED = new Standard.Win32Error(0x3fa);
            ERROR_NOT_FOUND = new Standard.Win32Error(0x490);
            ERROR_NO_MATCH = new Standard.Win32Error(0x491);
            ERROR_BAD_DEVICE = new Standard.Win32Error(0x4b0);
            ERROR_CANCELLED = new Standard.Win32Error(0x4c7);
            ERROR_CLASS_ALREADY_EXISTS = new Standard.Win32Error(0x582);
            ERROR_INVALID_DATATYPE = new Standard.Win32Error(0x70c);
        }

        public Win32Error(int i)
        {
            this._value = i;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return (((Standard.Win32Error) obj)._value == this._value);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this._value.GetHashCode();
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static Standard.Win32Error GetLastError()
        {
            return new Standard.Win32Error(Marshal.GetLastWin32Error());
        }

        public static bool operator ==(Standard.Win32Error errLeft, Standard.Win32Error errRight)
        {
            return (errLeft._value == errRight._value);
        }

        public static explicit operator Standard.HRESULT(Standard.Win32Error error)
        {
            if (error._value <= 0)
            {
                return new Standard.HRESULT((uint) error._value);
            }
            return Standard.HRESULT.Make(true, Standard.Facility.Win32, error._value & 0xffff);
        }

        public static bool operator !=(Standard.Win32Error errLeft, Standard.Win32Error errRight)
        {
            return !(errLeft == errRight);
        }

        public Standard.HRESULT ToHRESULT()
        {
            return (Standard.HRESULT) this;
        }
    }
}

