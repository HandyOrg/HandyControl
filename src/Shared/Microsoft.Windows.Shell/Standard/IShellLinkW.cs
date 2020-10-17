using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Standard
{
	// Token: 0x0200008F RID: 143
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IShellLinkW
	{
		// Token: 0x060001BD RID: 445
		void GetPath([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxPath, [In] [Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

		// Token: 0x060001BE RID: 446
		void GetIDList(out IntPtr ppidl);

		// Token: 0x060001BF RID: 447
		void SetIDList(IntPtr pidl);

		// Token: 0x060001C0 RID: 448
		void GetDescription([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxName);

		// Token: 0x060001C1 RID: 449
		void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x060001C2 RID: 450
		void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszDir, int cchMaxPath);

		// Token: 0x060001C3 RID: 451
		void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

		// Token: 0x060001C4 RID: 452
		void GetArguments([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszArgs, int cchMaxPath);

		// Token: 0x060001C5 RID: 453
		void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

		// Token: 0x060001C6 RID: 454
		short GetHotKey();

		// Token: 0x060001C7 RID: 455
		void SetHotKey(short wHotKey);

		// Token: 0x060001C8 RID: 456
		uint GetShowCmd();

		// Token: 0x060001C9 RID: 457
		void SetShowCmd(uint iShowCmd);

		// Token: 0x060001CA RID: 458
		void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

		// Token: 0x060001CB RID: 459
		void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

		// Token: 0x060001CC RID: 460
		void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

		// Token: 0x060001CD RID: 461
		void Resolve(IntPtr hwnd, uint fFlags);

		// Token: 0x060001CE RID: 462
		void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
	}
}
