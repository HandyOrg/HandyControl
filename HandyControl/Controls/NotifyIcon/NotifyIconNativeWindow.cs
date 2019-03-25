using System.Runtime.InteropServices;

namespace HandyControl.Controls
{
    internal class NotifyIconNativeWindow
    {
        internal NotifyIcon Reference;

        private GCHandle _rootRef;

        public void LockReference(bool locked)
        {
            if (locked)
            {
                if (!_rootRef.IsAllocated)
                {
                    _rootRef = GCHandle.Alloc(Reference, GCHandleType.Normal);
                }
            }
            else
            {
                if (_rootRef.IsAllocated)
                {
                    _rootRef.Free();
                }
            }
        }
    }
}