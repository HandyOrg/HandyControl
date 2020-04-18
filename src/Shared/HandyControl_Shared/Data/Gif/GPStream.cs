using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using HandyControl.Tools.Interop;

namespace HandyControl.Data
{
    // ReSharper disable once InconsistentNaming
    internal class GPStream : InteropValues.IStream
    {
        protected Stream DataStream;

        // to support seeking ahead of the stream length...
        long _virtualPosition = -1;

        internal GPStream(Stream stream)
        {
            if (!stream.CanSeek)
            {
                const int readBlock = 256;
                var bytes = new byte[readBlock];
                int readLen;
                var current = 0;
                do
                {
                    if (bytes.Length < current + readBlock)
                    {
                        var newData = new byte[bytes.Length * 2];
                        Array.Copy(bytes, newData, bytes.Length);
                        bytes = newData;
                    }
                    readLen = stream.Read(bytes, current, readBlock);
                    current += readLen;
                } while (readLen != 0);

                DataStream = new MemoryStream(bytes);
            }
            else
            {
                DataStream = stream;
            }
        }

        private void ActualizeVirtualPosition()
        {
            if (_virtualPosition == -1) return;

            if (_virtualPosition > DataStream.Length)
                DataStream.SetLength(_virtualPosition);

            DataStream.Position = _virtualPosition;

            _virtualPosition = -1;
        }

        public virtual InteropValues.IStream Clone()
        {
            NotImplemented();
            return null;
        }

        public virtual void Commit(int grfCommitFlags)
        {
            DataStream.Flush();
            // Extend the length of the file if needed.
            ActualizeVirtualPosition();
        }

        [
            UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows),
            SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)
        ]
        public virtual long CopyTo(InteropValues.IStream pstm, long cb, long[] pcbRead)
        {

            const int bufsize = 4096;
            var buffer = Marshal.AllocHGlobal(bufsize);
            if (buffer == IntPtr.Zero) throw new OutOfMemoryException();
            long written = 0;
            try
            {
                while (written < cb)
                {
                    var toRead = bufsize;
                    if (written + toRead > cb) toRead = (int)(cb - written);
                    var read = Read(buffer, toRead);
                    if (read == 0) break;
                    if (pstm.Write(buffer, read) != read)
                    {
                        throw EFail("Wrote an incorrect number of bytes");
                    }
                    written += read;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            if (pcbRead != null && pcbRead.Length > 0)
            {
                pcbRead[0] = written;
            }

            return written;
        }

        public virtual Stream GetDataStream()
        {
            return DataStream;
        }

        public virtual void LockRegion(long libOffset, long cb, int dwLockType)
        {
        }

        protected static ExternalException EFail(string msg)
        {
            throw new ExternalException(msg, InteropMethods.E_FAIL);
        }

        protected static void NotImplemented()
        {
            throw new ExternalException("NotImplemented");
        }

        public virtual int Read(IntPtr buf, int length)
        {
            var buffer = new byte[length];
            var count = Read(buffer, length);
            Marshal.Copy(buffer, 0, buf, length);
            return count;
        }

        public virtual int Read(byte[] buffer, int length)
        {
            ActualizeVirtualPosition();
            return DataStream.Read(buffer, 0, length);
        }

        public virtual void Revert()
        {
            NotImplemented();
        }

        public virtual long Seek(long offset, int origin)
        {
            var pos = _virtualPosition;
            if (_virtualPosition == -1)
            {
                pos = DataStream.Position;
            }
            var len = DataStream.Length;
            switch (origin)
            {
                case InteropValues.StreamConsts.STREAM_SEEK_SET:
                    if (offset <= len)
                    {
                        DataStream.Position = offset;
                        _virtualPosition = -1;
                    }
                    else
                    {
                        _virtualPosition = offset;
                    }
                    break;
                case InteropValues.StreamConsts.STREAM_SEEK_END:
                    if (offset <= 0)
                    {
                        DataStream.Position = len + offset;
                        _virtualPosition = -1;
                    }
                    else
                    {
                        _virtualPosition = len + offset;
                    }
                    break;
                case InteropValues.StreamConsts.STREAM_SEEK_CUR:
                    if (offset + pos <= len)
                    {
                        DataStream.Position = pos + offset;
                        _virtualPosition = -1;
                    }
                    else
                    {
                        _virtualPosition = offset + pos;
                    }
                    break;
            }
            return _virtualPosition != -1 ? _virtualPosition : DataStream.Position;
        }

        public virtual void SetSize(long value)
        {
            DataStream.SetLength(value);
        }

        public void Stat(IntPtr pstatstg, int grfStatFlag)
        {
            var stats = new STATSTG {cbSize = DataStream.Length};
            Marshal.StructureToPtr(stats, pstatstg, true);
        }

        public virtual void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
        }

        public virtual int Write(IntPtr buf, int length)
        {
            var buffer = new byte[length];
            Marshal.Copy(buf, buffer, 0, length);
            return Write(buffer, length);
        }

        public virtual int Write(byte[] buffer, int length)
        {
            ActualizeVirtualPosition();
            DataStream.Write(buffer, 0, length);
            return length;
        }

        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class STATSTG
        {

            [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
            public IntPtr pwcsName = IntPtr.Zero;
            public int type;
            [MarshalAs(UnmanagedType.I8)]
            public long cbSize;
            [MarshalAs(UnmanagedType.I8)]
            public long mtime;
            [MarshalAs(UnmanagedType.I8)]
            public long ctime;
            [MarshalAs(UnmanagedType.I8)]
            public long atime;
            [MarshalAs(UnmanagedType.I4)]
            public int grfMode;
            [MarshalAs(UnmanagedType.I4)]
            public int grfLocksSupported;

            public int clsid_data1;
            [MarshalAs(UnmanagedType.I2)]
            public short clsid_data2;
            [MarshalAs(UnmanagedType.I2)]
            public short clsid_data3;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b0;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b1;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b2;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b3;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b4;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b5;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b6;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b7;
            [MarshalAs(UnmanagedType.I4)]
            public int grfStateBits;
            [MarshalAs(UnmanagedType.I4)]
            public int reserved;
        }
    }
}