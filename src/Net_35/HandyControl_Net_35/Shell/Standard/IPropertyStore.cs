namespace Standard
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyStore
    {
        uint GetCount();
        Standard.PKEY GetAt(uint iProp);
        void GetValue([In] ref Standard.PKEY pkey, [In, Out] PROPVARIANT pv);
        void SetValue([In] ref Standard.PKEY pkey, PROPVARIANT pv);
        void Commit();
    }
}

