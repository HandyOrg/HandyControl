using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using HandyControl.Tools.Interop;

namespace HandyControl.Tools
{
    public static class VisualHelper
    {
        internal static VisualStateGroup TryGetVisualStateGroup(DependencyObject d, string groupName)
        {
            var root = GetImplementationRoot(d);
            if (root == null) return null;

            return VisualStateManager
                .GetVisualStateGroups(root)?
                .OfType<VisualStateGroup>()
                .FirstOrDefault(group => string.CompareOrdinal(groupName, group.Name) == 0);
        }

        internal static FrameworkElement GetImplementationRoot(DependencyObject d) =>
            1 == VisualTreeHelper.GetChildrenCount(d)
                ? VisualTreeHelper.GetChild(d, 0) as FrameworkElement
                : null;

        public static T GetChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d is T t) return t;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);

                var result = GetChild<T>(child);
                if (result != null) return result;
            }

            return default;
        }

        public static T GetParent<T>(DependencyObject d) where T : DependencyObject =>
            d switch
            {
                null => default,
                T t => t,
                Window _ => null,
                _ => GetParent<T>(VisualTreeHelper.GetParent(d))
            };

        public static IntPtr GetHandle(this Visual visual) => (PresentationSource.FromVisual(visual) as HwndSource)?.Handle ?? IntPtr.Zero;

        internal static void HitTestVisibleElements(Visual visual, HitTestResultCallback resultCallback, HitTestParameters parameters) =>
            VisualTreeHelper.HitTest(visual, ExcludeNonVisualElements, resultCallback, parameters);

        private static HitTestFilterBehavior ExcludeNonVisualElements(DependencyObject potentialHitTestTarget)
        {
            if (!(potentialHitTestTarget is Visual)) return HitTestFilterBehavior.ContinueSkipSelfAndChildren;

            if (!(potentialHitTestTarget is UIElement uIElement) || uIElement.IsVisible && uIElement.IsEnabled)
                return HitTestFilterBehavior.Continue;

            return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
        }

        internal static bool ModifyStyle(IntPtr hWnd, int styleToRemove, int styleToAdd)
        {
            var windowLong = InteropMethods.GetWindowLong(hWnd, InteropValues.GWL.STYLE);
            var num = (windowLong & ~styleToRemove) | styleToAdd;
            if (num == windowLong) return false;
            InteropMethods.SetWindowLong(hWnd, InteropValues.GWL.STYLE, num);
            return true;
        }
    }
}