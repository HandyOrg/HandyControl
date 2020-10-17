using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000091 RID: 145
	[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITaskbarList2 : ITaskbarList
	{
		// Token: 0x060001D4 RID: 468
		void HrInit();

		// Token: 0x060001D5 RID: 469
		void AddTab(IntPtr hwnd);

		// Token: 0x060001D6 RID: 470
		void DeleteTab(IntPtr hwnd);

		// Token: 0x060001D7 RID: 471
		void ActivateTab(IntPtr hwnd);

		// Token: 0x060001D8 RID: 472
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x060001D9 RID: 473
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
	}
}
