using System;

namespace Standard
{
	// Token: 0x02000055 RID: 85
	internal struct TITLEBARINFO
	{
		// Token: 0x04000448 RID: 1096
		public int cbSize;

		// Token: 0x04000449 RID: 1097
		public RECT rcTitleBar;

		// Token: 0x0400044A RID: 1098
		public STATE_SYSTEM rgstate_TitleBar;

		// Token: 0x0400044B RID: 1099
		public STATE_SYSTEM rgstate_Reserved;

		// Token: 0x0400044C RID: 1100
		public STATE_SYSTEM rgstate_MinimizeButton;

		// Token: 0x0400044D RID: 1101
		public STATE_SYSTEM rgstate_MaximizeButton;

		// Token: 0x0400044E RID: 1102
		public STATE_SYSTEM rgstate_HelpButton;

		// Token: 0x0400044F RID: 1103
		public STATE_SYSTEM rgstate_CloseButton;
	}
}
