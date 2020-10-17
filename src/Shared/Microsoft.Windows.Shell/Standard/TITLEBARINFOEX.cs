using System;

namespace Standard
{
	// Token: 0x02000056 RID: 86
	internal struct TITLEBARINFOEX
	{
		// Token: 0x04000450 RID: 1104
		public int cbSize;

		// Token: 0x04000451 RID: 1105
		public RECT rcTitleBar;

		// Token: 0x04000452 RID: 1106
		public STATE_SYSTEM rgstate_TitleBar;

		// Token: 0x04000453 RID: 1107
		public STATE_SYSTEM rgstate_Reserved;

		// Token: 0x04000454 RID: 1108
		public STATE_SYSTEM rgstate_MinimizeButton;

		// Token: 0x04000455 RID: 1109
		public STATE_SYSTEM rgstate_MaximizeButton;

		// Token: 0x04000456 RID: 1110
		public STATE_SYSTEM rgstate_HelpButton;

		// Token: 0x04000457 RID: 1111
		public STATE_SYSTEM rgstate_CloseButton;

		// Token: 0x04000458 RID: 1112
		public RECT rgrect_TitleBar;

		// Token: 0x04000459 RID: 1113
		public RECT rgrect_Reserved;

		// Token: 0x0400045A RID: 1114
		public RECT rgrect_MinimizeButton;

		// Token: 0x0400045B RID: 1115
		public RECT rgrect_MaximizeButton;

		// Token: 0x0400045C RID: 1116
		public RECT rgrect_HelpButton;

		// Token: 0x0400045D RID: 1117
		public RECT rgrect_CloseButton;
	}
}
