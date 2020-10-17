using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000089 RID: 137
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectCollection : IObjectArray
	{
		// Token: 0x0600018A RID: 394
		uint GetCount();

		// Token: 0x0600018B RID: 395
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);

		// Token: 0x0600018C RID: 396
		void AddObject([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x0600018D RID: 397
		void AddFromArray(IObjectArray poaSource);

		// Token: 0x0600018E RID: 398
		void RemoveObjectAt(uint uiIndex);

		// Token: 0x0600018F RID: 399
		void Clear();
	}
}
