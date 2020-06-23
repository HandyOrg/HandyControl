using System;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    public class GotoTop : Button
    {
        private Action _gotoTopAction;

        private System.Windows.Controls.ScrollViewer _scrollViewer;

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target", typeof(DependencyObject), typeof(GotoTop), new PropertyMetadata(default(DependencyObject)));

        public DependencyObject Target
        {
            get => (DependencyObject)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public GotoTop() => Loaded += (s, e) => CreateGotoAction(Target);

        public virtual void CreateGotoAction(DependencyObject obj)
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
            _scrollViewer = VisualHelper.GetChild<System.Windows.Controls.ScrollViewer>(obj);
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;

                if (_scrollViewer is ScrollViewer scrollViewerHandy && Animated && scrollViewerHandy.IsInertiaEnabled)
                {
                    _gotoTopAction = () => scrollViewerHandy.ScrollToTopInternal(AnimationTime);
                }
                else
                {
                    _gotoTopAction = () => _scrollViewer.ScrollToTop();
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (AutoHiding)
            {
                this.Show(e.VerticalOffset >= HidingHeight);
            }
        }

        public static readonly DependencyProperty AnimatedProperty = DependencyProperty.Register(
            "Animated", typeof(bool), typeof(GotoTop), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool Animated
        {
            get => (bool) GetValue(AnimatedProperty);
            set => SetValue(AnimatedProperty, value);
        }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register(
            "AnimationTime", typeof(double), typeof(GotoTop), new PropertyMetadata(ValueBoxes.Double200Box));

        public double AnimationTime
        {
            get => (double) GetValue(AnimationTimeProperty);
            set => SetValue(AnimationTimeProperty, value);
        }

        public static readonly DependencyProperty HidingHeightProperty = DependencyProperty.Register(
            "HidingHeight", typeof(double), typeof(GotoTop), new PropertyMetadata(ValueBoxes.Double0Box));

        public double HidingHeight
        {
            get => (double) GetValue(HidingHeightProperty);
            set => SetValue(HidingHeightProperty, value);
        }

        public static readonly DependencyProperty AutoHidingProperty = DependencyProperty.Register(
            "AutoHiding", typeof(bool), typeof(GotoTop), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool AutoHiding
        {
            get => (bool) GetValue(AutoHidingProperty);
            set => SetValue(AutoHidingProperty, value);
        }

        protected override void OnClick()
        {
            base.OnClick();

            _gotoTopAction?.Invoke();
        }
    }
}