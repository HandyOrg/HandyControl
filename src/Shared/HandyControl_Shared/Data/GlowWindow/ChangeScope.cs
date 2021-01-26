using HandyControl.Controls;

namespace HandyControl.Data
{
    internal class ChangeScope : DisposableObject
    {
        private readonly GlowWindow _window;

        public ChangeScope(GlowWindow window)
        {
            _window = window;
            _window.DeferGlowChangesCount++;
        }

        protected override void DisposeManagedResources()
        {
            _window.DeferGlowChangesCount--;
            if (_window.DeferGlowChangesCount == 0) _window.EndDeferGlowChanges();
        }
    }
}
