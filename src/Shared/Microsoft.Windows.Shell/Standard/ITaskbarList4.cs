using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000098 RID: 152
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[ComImport]
	internal interface ITaskbarList4 : ITaskbarList3, ITaskbarList2, ITaskbarList
	{
		// Token: 0x060001FE RID: 510
		void HrInit();

		// Token: 0x060001FF RID: 511
		void AddTab(IntPtr hwnd);

		// Token: 0x06000200 RID: 512
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06000201 RID: 513
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06000202 RID: 514
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x06000203 RID: 515
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		// Token: 0x06000204 RID: 516
		[PreserveSig]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		// Token: 0x06000205 RID: 517
		[PreserveSig]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		// Token: 0x06000206 RID: 518
		[PreserveSig]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		// Token: 0x06000207 RID: 519
		[PreserveSig]
		HRESULT UnregisterTab(IntPtr hwndTab);

		// Token: 0x06000208 RID: 520
		[PreserveSig]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		// Token: 0x06000209 RID: 521
		[PreserveSig]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		// Token: 0x0600020A RID: 522
		[PreserveSig]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x0600020B RID: 523
		[PreserveSig]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x0600020C RID: 524
		[PreserveSig]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		// Token: 0x0600020D RID: 525
		[PreserveSig]
		HRESULT SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		// Token: 0x0600020E RID: 526
		[PreserveSig]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		// Token: 0x0600020F RID: 527
		[PreserveSig]
		HRESULT SetThumbnailClip(IntPtr hwnd, RefRECT prcClip);

		// Token: 0x06000210 RID: 528
		void SetTabProperties(IntPtr hwndTab, STPF stpFlags);
	}
}
