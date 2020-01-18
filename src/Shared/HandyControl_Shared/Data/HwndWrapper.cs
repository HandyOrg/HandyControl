using System;
using System.Runtime.InteropServices;
using HandyControl.Tools.Interop;

namespace HandyControl.Data
{
    public abstract class HwndWrapper : DisposableObject
    {
        private IntPtr _handle;
        
        private bool _isHandleCreationAllowed = true;
        
        private ushort _wndClassAtom;
        
        private Delegate _wndProc;

        protected ushort WindowClassAtom
        {
            get
            {
                if (_wndClassAtom == 0) _wndClassAtom = CreateWindowClassCore();
                return _wndClassAtom;
            }
        }

        public IntPtr Handle
        {
            get
            {
                EnsureHandle();
                return _handle;
            }
        }

        protected virtual bool IsWindowSubclassed => false;

        protected virtual ushort CreateWindowClassCore() => RegisterClass(Guid.NewGuid().ToString());

        protected virtual void DestroyWindowClassCore()
        {
            if (_wndClassAtom != 0)
            {
                var moduleHandle = InteropMethods.GetModuleHandle(null);
                InteropMethods.UnregisterClass(new IntPtr(_wndClassAtom), moduleHandle);
                _wndClassAtom = 0;
            }
        }

        protected ushort RegisterClass(string className)
        {
            var wndClass = default(InteropValues.WNDCLASS);
            wndClass.cbClsExtra = 0;
            wndClass.cbWndExtra = 0;
            wndClass.hbrBackground = IntPtr.Zero;
            wndClass.hCursor = IntPtr.Zero;
            wndClass.hIcon = IntPtr.Zero;
            wndClass.lpfnWndProc = _wndProc = new InteropValues.WndProc(WndProc);
            wndClass.lpszClassName = className;
            wndClass.lpszMenuName = null;
            wndClass.style = 0u;
            return InteropMethods.RegisterClass(ref wndClass);
        }

        private void SubclassWndProc()
        {
            _wndProc = new InteropValues.WndProc(WndProc);
            InteropMethods.SetWindowLong(_handle, -4, Marshal.GetFunctionPointerForDelegate(_wndProc));
        }

        protected abstract IntPtr CreateWindowCore();

        protected virtual void DestroyWindowCore()
        {
            if (_handle != IntPtr.Zero)
            {
                InteropMethods.DestroyWindow(_handle);
                _handle = IntPtr.Zero;
            }
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam) =>
            InteropMethods.DefWindowProc(hwnd, msg, wParam, lParam);

        public void EnsureHandle()
        {
            if (_handle == IntPtr.Zero)
            {
                if (!_isHandleCreationAllowed) return;
                _isHandleCreationAllowed = false;
                _handle = CreateWindowCore();
                if (IsWindowSubclassed) SubclassWndProc();
            }
        }

        protected override void DisposeNativeResources()
        {
            _isHandleCreationAllowed = false;
            DestroyWindowCore();
            DestroyWindowClassCore();
        }
    }
}