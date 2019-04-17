namespace Standard
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices.ComTypes;

    internal sealed class SafeConnectionPointCookie : SafeHandleZeroOrMinusOneIsInvalid
    {
        private IConnectionPoint _cp;

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId="IConnectionPoint"), SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public SafeConnectionPointCookie(IConnectionPointContainer target, object sink, Guid eventId) : base(true)
        {
            Standard.Verify.IsNotNull<IConnectionPointContainer>(target, "target");
            Standard.Verify.IsNotNull<object>(sink, "sink");
            Standard.Verify.IsNotDefault<Guid>(eventId, "eventId");
            base.handle = IntPtr.Zero;
            IConnectionPoint ppCP = null;
            try
            {
                int num;
                target.FindConnectionPoint(ref eventId, out ppCP);
                ppCP.Advise(sink, out num);
                if (num == 0)
                {
                    throw new InvalidOperationException("IConnectionPoint::Advise returned an invalid cookie.");
                }
                base.handle = new IntPtr(num);
                this._cp = ppCP;
                ppCP = null;
            }
            finally
            {
                Standard.Utility.SafeRelease<IConnectionPoint>(ref ppCP);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void Disconnect()
        {
            this.ReleaseHandle();
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            try
            {
                if (!this.IsInvalid)
                {
                    int dwCookie = this.handle.ToInt32();
                    base.handle = IntPtr.Zero;
                    try
                    {
                        this._cp.Unadvise(dwCookie);
                    }
                    finally
                    {
                        Standard.Utility.SafeRelease<IConnectionPoint>(ref this._cp);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

