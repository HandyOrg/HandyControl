namespace Standard
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit), SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class PROPVARIANT : IDisposable
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields"), FieldOffset(8)]
        private short boolVal;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields"), FieldOffset(8)]
        private byte byteVal;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields"), FieldOffset(8)]
        private long longVal;
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources"), SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields"), FieldOffset(8)]
        private IntPtr pointerVal;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields"), FieldOffset(0)]
        private ushort vt;

        public void Clear()
        {
            NativeMethods.PropVariantClear(this);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId="disposing")]
        private void Dispose(bool disposing)
        {
            this.Clear();
        }

        ~PROPVARIANT()
        {
            this.Dispose(false);
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public string GetValue()
        {
            if (this.vt == 0x1f)
            {
                return Marshal.PtrToStringUni(this.pointerVal);
            }
            return null;
        }

        public void SetValue(bool f)
        {
            this.Clear();
            this.vt = 11;
            this.boolVal = f ? ((short) (-1)) : ((short) 0);
        }

        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public void SetValue(string val)
        {
            this.Clear();
            this.vt = 0x1f;
            this.pointerVal = Marshal.StringToCoTaskMemUni(val);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public VarEnum VarType
        {
            get
            {
                return (VarEnum) this.vt;
            }
        }

        private static class NativeMethods
        {
            [DllImport("ole32.dll")]
            internal static extern Standard.HRESULT PropVariantClear(PROPVARIANT pvar);
        }
    }
}

