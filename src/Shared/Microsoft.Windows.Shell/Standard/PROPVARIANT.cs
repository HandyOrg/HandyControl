using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000058 RID: 88
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[StructLayout(LayoutKind.Explicit)]
	internal class PROPVARIANT : IDisposable
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004649 File Offset: 0x00002849
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		public VarEnum VarType
		{
			get
			{
				return (VarEnum)this.vt;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004651 File Offset: 0x00002851
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public string GetValue()
		{
			if (this.vt == 31)
			{
				return Marshal.PtrToStringUni(this.pointerVal);
			}
			return null;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000466A File Offset: 0x0000286A
		public void SetValue(bool f)
		{
			this.Clear();
			this.vt = 11;
			this.boolVal = (short)(f ? -1 : 0);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004688 File Offset: 0x00002888
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public void SetValue(string val)
		{
			this.Clear();
			this.vt = 31;
			this.pointerVal = Marshal.StringToCoTaskMemUni(val);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000046A4 File Offset: 0x000028A4
		public void Clear()
		{
			PROPVARIANT.NativeMethods.PropVariantClear(this);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000046AD File Offset: 0x000028AD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000046BC File Offset: 0x000028BC
		~PROPVARIANT()
		{
			this.Dispose(false);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000046EC File Offset: 0x000028EC
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "disposing")]
		private void Dispose(bool disposing)
		{
			this.Clear();
		}

		// Token: 0x0400046D RID: 1133
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		[FieldOffset(0)]
		private ushort vt;

		// Token: 0x0400046E RID: 1134
		[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		[FieldOffset(8)]
		private IntPtr pointerVal;

		// Token: 0x0400046F RID: 1135
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		[FieldOffset(8)]
		private byte byteVal;

		// Token: 0x04000470 RID: 1136
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		[FieldOffset(8)]
		private long longVal;

		// Token: 0x04000471 RID: 1137
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		[FieldOffset(8)]
		private short boolVal;

		// Token: 0x02000059 RID: 89
		private static class NativeMethods
		{
			// Token: 0x060000C3 RID: 195
			[DllImport("ole32.dll")]
			internal static extern HRESULT PropVariantClear(PROPVARIANT pvar);
		}
	}
}
