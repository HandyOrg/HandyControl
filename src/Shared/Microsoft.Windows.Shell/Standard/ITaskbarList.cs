using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000090 RID: 144
	[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITaskbarList
	{
		// Token: 0x060001CF RID: 463
		void HrInit();

		// Token: 0x060001D0 RID: 464
		void AddTab(IntPtr hwnd);

		// Token: 0x060001D1 RID: 465
		void DeleteTab(IntPtr hwnd);

		// Token: 0x060001D2 RID: 466
		void ActivateTab(IntPtr hwnd);

		// Token: 0x060001D3 RID: 467
		void SetActiveAlt(IntPtr hwnd);
	}
}
