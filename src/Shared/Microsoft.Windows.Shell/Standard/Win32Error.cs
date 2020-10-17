using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000012 RID: 18
	[StructLayout(LayoutKind.Explicit)]
	internal struct Win32Error
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00003945 File Offset: 0x00001B45
		public Win32Error(int i)
		{
			this._value = i;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000394E File Offset: 0x00001B4E
		public static explicit operator HRESULT(Win32Error error)
		{
			if (error._value <= 0)
			{
				return new HRESULT((uint)error._value);
			}
			return HRESULT.Make(true, Facility.Win32, error._value & 65535);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000397B File Offset: 0x00001B7B
		public HRESULT ToHRESULT()
		{
			return (HRESULT)this;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003988 File Offset: 0x00001B88
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public static Win32Error GetLastError()
		{
			return new Win32Error(Marshal.GetLastWin32Error());
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003994 File Offset: 0x00001B94
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				result = (((Win32Error)obj)._value == this._value);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000039D0 File Offset: 0x00001BD0
		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000039EB File Offset: 0x00001BEB
		public static bool operator ==(Win32Error errLeft, Win32Error errRight)
		{
			return errLeft._value == errRight._value;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000039FD File Offset: 0x00001BFD
		public static bool operator !=(Win32Error errLeft, Win32Error errRight)
		{
			return !(errLeft == errRight);
		}

		// Token: 0x04000041 RID: 65
		[FieldOffset(0)]
		private readonly int _value;

		// Token: 0x04000042 RID: 66
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_SUCCESS = new Win32Error(0);

		// Token: 0x04000043 RID: 67
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_INVALID_FUNCTION = new Win32Error(1);

		// Token: 0x04000044 RID: 68
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_FILE_NOT_FOUND = new Win32Error(2);

		// Token: 0x04000045 RID: 69
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_PATH_NOT_FOUND = new Win32Error(3);

		// Token: 0x04000046 RID: 70
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_TOO_MANY_OPEN_FILES = new Win32Error(4);

		// Token: 0x04000047 RID: 71
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_ACCESS_DENIED = new Win32Error(5);

		// Token: 0x04000048 RID: 72
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_INVALID_HANDLE = new Win32Error(6);

		// Token: 0x04000049 RID: 73
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_OUTOFMEMORY = new Win32Error(14);

		// Token: 0x0400004A RID: 74
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_NO_MORE_FILES = new Win32Error(18);

		// Token: 0x0400004B RID: 75
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_SHARING_VIOLATION = new Win32Error(32);

		// Token: 0x0400004C RID: 76
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_INVALID_PARAMETER = new Win32Error(87);

		// Token: 0x0400004D RID: 77
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_INSUFFICIENT_BUFFER = new Win32Error(122);

		// Token: 0x0400004E RID: 78
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_NESTING_NOT_ALLOWED = new Win32Error(215);

		// Token: 0x0400004F RID: 79
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_KEY_DELETED = new Win32Error(1018);

		// Token: 0x04000050 RID: 80
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_NOT_FOUND = new Win32Error(1168);

		// Token: 0x04000051 RID: 81
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_NO_MATCH = new Win32Error(1169);

		// Token: 0x04000052 RID: 82
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_BAD_DEVICE = new Win32Error(1200);

		// Token: 0x04000053 RID: 83
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_CANCELLED = new Win32Error(1223);

		// Token: 0x04000054 RID: 84
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_CLASS_ALREADY_EXISTS = new Win32Error(1410);

		// Token: 0x04000055 RID: 85
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly Win32Error ERROR_INVALID_DATATYPE = new Win32Error(1804);
	}
}
