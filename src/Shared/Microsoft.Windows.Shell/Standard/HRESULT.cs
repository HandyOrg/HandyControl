using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000014 RID: 20
	[StructLayout(LayoutKind.Explicit)]
	internal struct HRESULT
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00003B1A File Offset: 0x00001D1A
		public HRESULT(uint i)
		{
			this._value = i;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003B23 File Offset: 0x00001D23
		public static HRESULT Make(bool severe, Facility facility, int code)
		{
			return new HRESULT((uint)((severe ? ((Facility)(-2147483648)) : Facility.Null) | (Facility)((int)facility << 16) | (Facility)code));
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003B3C File Offset: 0x00001D3C
		public Facility Facility
		{
			get
			{
				return HRESULT.GetFacility((int)this._value);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003B49 File Offset: 0x00001D49
		public static Facility GetFacility(int errorCode)
		{
			return (Facility)(errorCode >> 16 & 8191);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003B55 File Offset: 0x00001D55
		public int Code
		{
			get
			{
				return HRESULT.GetCode((int)this._value);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003B62 File Offset: 0x00001D62
		public static int GetCode(int error)
		{
			return error & 65535;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003B6C File Offset: 0x00001D6C
		public override string ToString()
		{
			foreach (FieldInfo fieldInfo in typeof(HRESULT).GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				if (fieldInfo.FieldType == typeof(HRESULT))
				{
					HRESULT hrLeft = (HRESULT)fieldInfo.GetValue(null);
					if (hrLeft == this)
					{
						return fieldInfo.Name;
					}
				}
			}
			if (this.Facility == Facility.Win32)
			{
				foreach (FieldInfo fieldInfo2 in typeof(Win32Error).GetFields(BindingFlags.Static | BindingFlags.Public))
				{
					if (fieldInfo2.FieldType == typeof(Win32Error))
					{
						Win32Error error = (Win32Error)fieldInfo2.GetValue(null);
						if ((HRESULT)error == this)
						{
							return "HRESULT_FROM_WIN32(" + fieldInfo2.Name + ")";
						}
					}
				}
			}
			return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", new object[]
			{
				this._value
			});
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003C94 File Offset: 0x00001E94
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				result = (((HRESULT)obj)._value == this._value);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003CEB File Offset: 0x00001EEB
		public static bool operator ==(HRESULT hrLeft, HRESULT hrRight)
		{
			return hrLeft._value == hrRight._value;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003CFD File Offset: 0x00001EFD
		public static bool operator !=(HRESULT hrLeft, HRESULT hrRight)
		{
			return !(hrLeft == hrRight);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003D09 File Offset: 0x00001F09
		public bool Succeeded
		{
			get
			{
				return this._value >= 0U;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003D17 File Offset: 0x00001F17
		public bool Failed
		{
			get
			{
				return this._value < 0U;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003D22 File Offset: 0x00001F22
		public void ThrowIfFailed()
		{
			this.ThrowIfFailed(null);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003D2C File Offset: 0x00001F2C
		[SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Only recreating Exceptions that were already raised.")]
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public void ThrowIfFailed(string message)
		{
			if (this.Failed)
			{
				if (string.IsNullOrEmpty(message))
				{
					message = this.ToString();
				}
				Exception ex = Marshal.GetExceptionForHR((int)this._value, new IntPtr(-1));
				if (ex.GetType() == typeof(COMException))
				{
					Facility facility = this.Facility;
					if (facility == Facility.Win32)
					{
						ex = new Win32Exception(this.Code, message);
					}
					else
					{
						ex = new COMException(message, (int)this._value);
					}
				}
				else
				{
					ConstructorInfo constructor = ex.GetType().GetConstructor(new Type[]
					{
						typeof(string)
					});
					if (null != constructor)
					{
						ex = (constructor.Invoke(new object[]
						{
							message
						}) as Exception);
					}
				}
				throw ex;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003DF4 File Offset: 0x00001FF4
		public static void ThrowLastError()
		{
			((HRESULT)Win32Error.GetLastError()).ThrowIfFailed();
		}

		// Token: 0x04000061 RID: 97
		[FieldOffset(0)]
		private readonly uint _value;

		// Token: 0x04000062 RID: 98
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT S_OK = new HRESULT(0U);

		// Token: 0x04000063 RID: 99
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT S_FALSE = new HRESULT(1U);

		// Token: 0x04000064 RID: 100
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_PENDING = new HRESULT(2147483658U);

		// Token: 0x04000065 RID: 101
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_NOTIMPL = new HRESULT(2147500033U);

		// Token: 0x04000066 RID: 102
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_NOINTERFACE = new HRESULT(2147500034U);

		// Token: 0x04000067 RID: 103
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_POINTER = new HRESULT(2147500035U);

		// Token: 0x04000068 RID: 104
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_ABORT = new HRESULT(2147500036U);

		// Token: 0x04000069 RID: 105
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_FAIL = new HRESULT(2147500037U);

		// Token: 0x0400006A RID: 106
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_UNEXPECTED = new HRESULT(2147549183U);

		// Token: 0x0400006B RID: 107
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT STG_E_INVALIDFUNCTION = new HRESULT(2147680257U);

		// Token: 0x0400006C RID: 108
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT REGDB_E_CLASSNOTREG = new HRESULT(2147746132U);

		// Token: 0x0400006D RID: 109
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER = new HRESULT(2147749635U);

		// Token: 0x0400006E RID: 110
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT DESTS_E_NORECDOCS = new HRESULT(2147749636U);

		// Token: 0x0400006F RID: 111
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT DESTS_E_NOTALLCLEARED = new HRESULT(2147749637U);

		// Token: 0x04000070 RID: 112
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_ACCESSDENIED = new HRESULT(2147942405U);

		// Token: 0x04000071 RID: 113
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_OUTOFMEMORY = new HRESULT(2147942414U);

		// Token: 0x04000072 RID: 114
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT E_INVALIDARG = new HRESULT(2147942487U);

		// Token: 0x04000073 RID: 115
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW = new HRESULT(2147942934U);

		// Token: 0x04000074 RID: 116
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT COR_E_OBJECTDISPOSED = new HRESULT(2148734498U);

		// Token: 0x04000075 RID: 117
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT WC_E_GREATERTHAN = new HRESULT(3222072867U);

		// Token: 0x04000076 RID: 118
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly HRESULT WC_E_SYNTAX = new HRESULT(3222072877U);
	}
}
