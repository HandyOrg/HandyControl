using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Standard;

[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
[StructLayout(LayoutKind.Explicit)]
internal class PROPVARIANT : IDisposable
{
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public VarEnum VarType
    {
        get
        {
            return (VarEnum) this.vt;
        }
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    public string GetValue()
    {
        if (this.vt == 31)
        {
            return Marshal.PtrToStringUni(this.pointerVal);
        }
        return null;
    }

    public void SetValue(bool f)
    {
        this.Clear();
        this.vt = 11;
        this.boolVal = (short) (f ? -1 : 0);
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    public void SetValue(string val)
    {
        this.Clear();
        this.vt = 31;
        this.pointerVal = Marshal.StringToCoTaskMemUni(val);
    }

    public void Clear()
    {
        PROPVARIANT.NativeMethods.PropVariantClear(this);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~PROPVARIANT()
    {
        this.Dispose(false);
    }

    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "disposing")]
    private void Dispose(bool disposing)
    {
        this.Clear();
    }

    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    [FieldOffset(0)]
    private ushort vt;

    [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    [FieldOffset(8)]
    private IntPtr pointerVal;

    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    [FieldOffset(8)]
    private byte byteVal;

    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    [FieldOffset(8)]
    private long longVal;

    [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
    [FieldOffset(8)]
    private short boolVal;

    private static class NativeMethods
    {
        [DllImport("ole32.dll")]
        internal static extern HRESULT PropVariantClear(PROPVARIANT pvar);
    }
}
