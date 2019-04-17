using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    [TemplateVisualState(GroupName = PresentationGroup, Name = NormalState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = DefaultTransitionState)]
    [TemplatePart(Name = PreviousContentPresentationSitePartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = CurrentContentPresentationSitePartName, Type = typeof(ContentControl))]
    public class TransitioningContentControl : ContentControl
    {
        private Storyboard _currentTransition;

        public TransitioningContentControl()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DefaultStyleKey = typeof(TransitioningContentControl);
            }
        }

        private Storyboard CurrentTransition
        {
            set
            {
                if (_currentTransition != null)
                    _currentTransition.Completed -= OnTransitionCompleted;

                _currentTransition = value;

                if (_currentTransition != null)
                    _currentTransition.Completed += OnTransitionCompleted;
            }
        }

        #region Events

        public event RoutedEventHandler TransitionCompleted;

        #endregion Events

        public override void OnApplyTemplate()
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            if (IsTransitioning)
                AbortTransition();

            base.OnApplyTemplate();

            PreviousContentPresentationSite =
                GetTemplateChild(PreviousContentPresentationSitePartName) as ContentPresenter;
            CurrentContentPresentationSite =
                GetTemplateChild(CurrentContentPresentationSitePartName) as ContentPresenter;

            if (CurrentContentPresentationSite != null)
                CurrentContentPresentationSite.Content = Content;

            var transition = GetStoryboard(Transition);
            CurrentTransition = transition;
            if (transition == null)
            {
                Transition = DefaultTransitionState;

                throw new ArgumentException("TransitioningContentControl_TransitionNotFound");
            }

            VisualStateManager.GoToState(this, NormalState, false);
            VisualStateManager.GoToState(this, Transition, true);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            StartTransition(oldContent, newContent);
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "newContent", Justification = "Should be used in the future.")]
        private void StartTransition(object oldContent, object newContent)
        {
            if (CurrentContentPresentationSite != null && PreviousContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = newContent;

                PreviousContentPresentationSite.Content = oldContent;

                if (!IsTransitioning || RestartTransitionOnContentChange)
                {
                    IsTransitioning = true;
                    VisualStateManager.GoToState(this, NormalState, false);
                    VisualStateManager.GoToState(this, Transition, true);
                }
            }
        }

        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            AbortTransition();

            var handler = TransitionCompleted;
            handler?.Invoke(this, new RoutedEventArgs());
        }

        public void AbortTransition()
        {
            VisualStateManager.GoToState(this, NormalState, false);
            IsTransitioning = false;
            if (PreviousContentPresentationSite != null)
                PreviousContentPresentationSite.Content = null;
        }

        private Storyboard GetStoryboard(string newTransition)
        {
            var presentationGroup = VisualHelper.TryGetVisualStateGroup(this, PresentationGroup);
            Storyboard newStoryboard = null;
            if (presentationGroup != null)
                newStoryboard = presentationGroup.States
                    .OfType<VisualState>()
                    .Where(state => state.Name == newTransition)
                    .Select(state => state.Storyboard)
                    .FirstOrDefault();
            return newStoryboard;
        }

        #region Visual state names

        private const string PresentationGroup = "PresentationStates";

        private const string NormalState = "Normal";

        public const string DefaultTransitionState = "DefaultTransition";

        #endregion Visual state names

        #region Template part names

        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";

        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        #endregion Template part names

        #region TemplateParts

        private ContentPresenter CurrentContentPresentationSite { get; set; }

        private ContentPresenter PreviousContentPresentationSite { get; set; }

        #endregion TemplateParts

        #region public bool IsTransitioning

        private bool _allowIsTransitioningWrite;

        public bool IsTransitioning
        {
            get => (bool)GetValue(IsTransitioningProperty);
            private set
            {
                _allowIsTransitioningWrite = true;
                SetValue(IsTransitioningProperty, value);
                _allowIsTransitioningWrite = false;
            }
        }

        public static readonly DependencyProperty IsTransitioningProperty =
            DependencyProperty.Register(
                "IsTransitioning",
                typeof(bool),
                typeof(TransitioningContentControl),
                new PropertyMetadata(OnIsTransitioningPropertyChanged));

        private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TransitioningContentControl)d;

            if (!source._allowIsTransitioningWrite)
            {
                source.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException("TransitiotioningContentControl_IsTransitioningReadOnly");
            }
        }

        #endregion public bool IsTransitioning

        #region public string Transition

        public string Transition
        {
            get => GetValue(TransitionProperty) as string;
            set => SetValue(TransitionProperty, value);
        }

        public static readonly DependencyProperty TransitionProperty =
            DependencyProperty.Register(
                "Transition",
                typeof(string),
                typeof(TransitioningContentControl),
                new PropertyMetadata(DefaultTransitionState, OnTransitionPropertyChanged));

        private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TransitioningContentControl)d;
            var oldTransition = e.OldValue as string;
            var newTransition = e.NewValue as string;

            if (source.IsTransitioning)
                source.AbortTransition();

            var newStoryboard = source.GetStoryboard(newTransition);

            if (newStoryboard == null)
                if (VisualHelper.TryGetVisualStateGroup(source, PresentationGroup) == null)
                {
                    source.CurrentTransition = null;
                }
                else
                {
                    source.SetValue(TransitionProperty, oldTransition);

                    throw new ArgumentException(
                        "TransitioningContentControl_TransitionNotFound");
                }
            else
                source.CurrentTransition = newStoryboard;
        }

        #endregion public string Transition

        #region public bool RestartTransitionOnContentChange

        public bool RestartTransitionOnContentChange
        {
            get => (bool)GetValue(RestartTransitionOnContentChangeProperty);
            set => SetValue(RestartTransitionOnContentChangeProperty, value);
        }

        public static readonly DependencyProperty RestartTransitionOnContentChangeProperty =
            DependencyProperty.Register(
                "RestartTransitionOnContentChange",
                typeof(bool),
                typeof(TransitioningContentControl),
                new PropertyMetadata(ValueBoxes.FalseBox, OnRestartTransitionOnContentChangePropertyChanged));

        private static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ((TransitioningContentControl)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue,
                (bool)e.NewValue);
        }

        protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
        {
        }

        #endregion public bool RestartTransitionOnContentChange
    }
}