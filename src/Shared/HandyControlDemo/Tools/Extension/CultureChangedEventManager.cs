using System;
using System.Windows;

namespace HandyControlDemo.Tools.Extension

{
    public class CultureChangedEventManager : WeakEventManager
    {
        private static CultureChangedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(CultureChangedEventManager);
                var manager = (CultureChangedEventManager)GetCurrentManager(managerType);
                if (manager == null)
                {
                    manager = new CultureChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }
                return manager;
            }
        }

        public static void AddListener(LocalizationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(LocalizationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        private void OnCultureChanged(object sender, EventArgs e)
        {
            DeliverEvent(sender, e);
        }

        protected override void StartListening(object source)
        {
            var manager = (LocalizationManager)source;
            manager.CultureChanged += OnCultureChanged;
        }

        protected override void StopListening(object source)
        {
            var manager = (LocalizationManager)source;
            manager.CultureChanged -= OnCultureChanged;
        }
    }
}
