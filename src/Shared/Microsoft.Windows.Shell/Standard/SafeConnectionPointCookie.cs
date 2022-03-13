using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32.SafeHandles;

namespace Standard;

internal sealed class SafeConnectionPointCookie : SafeHandleZeroOrMinusOneIsInvalid
{
    [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IConnectionPoint")]
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public SafeConnectionPointCookie(IConnectionPointContainer target, object sink, Guid eventId) : base(true)
    {
        Verify.IsNotNull<IConnectionPointContainer>(target, "target");
        Verify.IsNotNull<object>(sink, "sink");
        Verify.IsNotDefault<Guid>(eventId, "eventId");
        this.handle = IntPtr.Zero;
        IConnectionPoint connectionPoint = null;
        try
        {
            target.FindConnectionPoint(ref eventId, out connectionPoint);
            int num;
            connectionPoint.Advise(sink, out num);
            if (num == 0)
            {
                throw new InvalidOperationException("IConnectionPoint::Advise returned an invalid cookie.");
            }
            this.handle = new IntPtr(num);
            this._cp = connectionPoint;
            connectionPoint = null;
        }
        finally
        {
            Utility.SafeRelease<IConnectionPoint>(ref connectionPoint);
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public void Disconnect()
    {
        this.ReleaseHandle();
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
        bool result;
        try
        {
            if (!this.IsInvalid)
            {
                int dwCookie = this.handle.ToInt32();
                this.handle = IntPtr.Zero;
                try
                {
                    this._cp.Unadvise(dwCookie);
                }
                finally
                {
                    Utility.SafeRelease<IConnectionPoint>(ref this._cp);
                }
            }
            result = true;
        }
        catch
        {
            result = false;
        }
        return result;
    }

    private IConnectionPoint _cp;
}
