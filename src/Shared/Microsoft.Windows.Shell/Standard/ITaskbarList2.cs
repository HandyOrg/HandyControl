using System;
using System.Runtime.InteropServices;

namespace Standard;

[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface ITaskbarList2 : ITaskbarList
{
    void HrInit();

    void AddTab(IntPtr hwnd);

    void DeleteTab(IntPtr hwnd);

    void ActivateTab(IntPtr hwnd);

    void SetActiveAlt(IntPtr hwnd);

    void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
}
