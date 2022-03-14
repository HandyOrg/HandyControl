using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard;

internal sealed class ManagedIStream : IStream, IDisposable
{
    public ManagedIStream(Stream source)
    {
        Verify.IsNotNull<Stream>(source, "source");
        this._source = source;
    }

    private void _Validate()
    {
        if (this._source == null)
        {
            throw new ObjectDisposedException("this");
        }
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
    [Obsolete("The method is not implemented", true)]
    public void Clone(out IStream ppstm)
    {
        ppstm = null;
        HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    public void Commit(int grfCommitFlags)
    {
        this._Validate();
        this._source.Flush();
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
    public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
    {
        Verify.IsNotNull<IStream>(pstm, "pstm");
        this._Validate();
        byte[] array = new byte[4096];
        long num;
        int num2;
        for (num = 0L; num < cb; num += (long) num2)
        {
            num2 = this._source.Read(array, 0, array.Length);
            if (num2 == 0)
            {
                break;
            }
            pstm.Write(array, num2, IntPtr.Zero);
        }
        if (IntPtr.Zero != pcbRead)
        {
            Marshal.WriteInt64(pcbRead, num);
        }
        if (IntPtr.Zero != pcbWritten)
        {
            Marshal.WriteInt64(pcbWritten, num);
        }
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
    [Obsolete("The method is not implemented", true)]
    public void LockRegion(long libOffset, long cb, int dwLockType)
    {
        HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    public void Read(byte[] pv, int cb, IntPtr pcbRead)
    {
        this._Validate();
        int val = this._source.Read(pv, 0, cb);
        if (IntPtr.Zero != pcbRead)
        {
            Marshal.WriteInt32(pcbRead, val);
        }
    }

    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
    [Obsolete("The method is not implemented", true)]
    public void Revert()
    {
        HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
    {
        this._Validate();
        long val = this._source.Seek(dlibMove, (SeekOrigin) dwOrigin);
        if (IntPtr.Zero != plibNewPosition)
        {
            Marshal.WriteInt64(plibNewPosition, val);
        }
    }

    public void SetSize(long libNewSize)
    {
        this._Validate();
        this._source.SetLength(libNewSize);
    }

    public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
    {
        pstatstg = default(System.Runtime.InteropServices.ComTypes.STATSTG);
        this._Validate();
        pstatstg.type = 2;
        pstatstg.cbSize = this._source.Length;
        pstatstg.grfMode = 2;
        pstatstg.grfLocksSupported = 2;
    }

    [Obsolete("The method is not implemented", true)]
    [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
    public void UnlockRegion(long libOffset, long cb, int dwLockType)
    {
        HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
    }

    [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
    public void Write(byte[] pv, int cb, IntPtr pcbWritten)
    {
        this._Validate();
        this._source.Write(pv, 0, cb);
        if (IntPtr.Zero != pcbWritten)
        {
            Marshal.WriteInt32(pcbWritten, cb);
        }
    }

    public void Dispose()
    {
        this._source = null;
    }

    private const int STGTY_STREAM = 2;

    private const int STGM_READWRITE = 2;

    private const int LOCK_EXCLUSIVE = 2;

    private Stream _source;
}
