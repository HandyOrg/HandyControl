namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    internal struct PKEY
    {
        private readonly Guid _fmtid;
        private readonly uint _pid;
        public static readonly Standard.PKEY Title;
        public static readonly Standard.PKEY AppUserModel_ID;
        public static readonly Standard.PKEY AppUserModel_IsDestListSeparator;
        public static readonly Standard.PKEY AppUserModel_RelaunchCommand;
        public static readonly Standard.PKEY AppUserModel_RelaunchDisplayNameResource;
        public static readonly Standard.PKEY AppUserModel_RelaunchIconResource;
        public PKEY(Guid fmtid, uint pid)
        {
            this._fmtid = fmtid;
            this._pid = pid;
        }

        static PKEY()
        {
            Title = new Standard.PKEY(new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2);
            AppUserModel_ID = new Standard.PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);
            AppUserModel_IsDestListSeparator = new Standard.PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 6);
            AppUserModel_RelaunchCommand = new Standard.PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 2);
            AppUserModel_RelaunchDisplayNameResource = new Standard.PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 4);
            AppUserModel_RelaunchIconResource = new Standard.PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 3);
        }
    }
}

