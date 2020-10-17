using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x02000099 RID: 153
	internal sealed class ManagedIStream : IStream, IDisposable
	{
		// Token: 0x06000211 RID: 529 RVA: 0x000052FF File Offset: 0x000034FF
		public ManagedIStream(Stream source)
		{
			Verify.IsNotNull<Stream>(source, "source");
			this._source = source;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00005319 File Offset: 0x00003519
		private void _Validate()
		{
			if (this._source == null)
			{
				throw new ObjectDisposedException("this");
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00005330 File Offset: 0x00003530
		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
		[Obsolete("The method is not implemented", true)]
		public void Clone(out IStream ppstm)
		{
			ppstm = null;
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00005352 File Offset: 0x00003552
		public void Commit(int grfCommitFlags)
		{
			this._Validate();
			this._source.Flush();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00005368 File Offset: 0x00003568
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
		{
			Verify.IsNotNull<IStream>(pstm, "pstm");
			this._Validate();
			byte[] array = new byte[4096];
			long num;
			int num2;
			for (num = 0L; num < cb; num += (long)num2)
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

		// Token: 0x06000216 RID: 534 RVA: 0x000053EC File Offset: 0x000035EC
		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
		[Obsolete("The method is not implemented", true)]
		public void LockRegion(long libOffset, long cb, int dwLockType)
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000540C File Offset: 0x0000360C
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

		// Token: 0x06000218 RID: 536 RVA: 0x00005444 File Offset: 0x00003644
		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
		[Obsolete("The method is not implemented", true)]
		public void Revert()
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00005464 File Offset: 0x00003664
		[SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
		public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
		{
			this._Validate();
			long val = this._source.Seek(dlibMove, (SeekOrigin)dwOrigin);
			if (IntPtr.Zero != plibNewPosition)
			{
				Marshal.WriteInt64(plibNewPosition, val);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00005499 File Offset: 0x00003699
		public void SetSize(long libNewSize)
		{
			this._Validate();
			this._source.SetLength(libNewSize);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000054AD File Offset: 0x000036AD
		public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
		{
			pstatstg = default(System.Runtime.InteropServices.ComTypes.STATSTG);
			this._Validate();
			pstatstg.type = 2;
			pstatstg.cbSize = this._source.Length;
			pstatstg.grfMode = 2;
			pstatstg.grfLocksSupported = 2;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000054E4 File Offset: 0x000036E4
		[Obsolete("The method is not implemented", true)]
		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.HRESULT.ThrowIfFailed(System.String)")]
		public void UnlockRegion(long libOffset, long cb, int dwLockType)
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00005503 File Offset: 0x00003703
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

		// Token: 0x0600021E RID: 542 RVA: 0x0000552D File Offset: 0x0000372D
		public void Dispose()
		{
			this._source = null;
		}

		// Token: 0x04000596 RID: 1430
		private const int STGTY_STREAM = 2;

		// Token: 0x04000597 RID: 1431
		private const int STGM_READWRITE = 2;

		// Token: 0x04000598 RID: 1432
		private const int LOCK_EXCLUSIVE = 2;

		// Token: 0x04000599 RID: 1433
		private Stream _source;
	}
}
