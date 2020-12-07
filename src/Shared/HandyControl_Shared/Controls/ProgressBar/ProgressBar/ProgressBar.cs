// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HandyControl.Controls.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HandyControl.Controls
{
    /// <summary>
    /// Represents a control that indicates the progress of an operation, where the typical
    /// visual appearance is a bar that animates a filled area as progress continues.
    /// </summary>
    [TemplatePart(Name = s_LayoutRootName, Type = typeof(Grid))]
    [TemplatePart(Name = s_DeterminateProgressBarIndicatorName, Type = typeof(Rectangle))]
    [TemplatePart(Name = s_IndeterminateProgressBarIndicatorName, Type = typeof(Rectangle))]
    [TemplatePart(Name = s_IndeterminateProgressBarIndicator2Name, Type = typeof(Rectangle))]
    [TemplateVisualState(GroupName = GroupCommon, Name = s_DeterminateStateName)]
    [TemplateVisualState(GroupName = GroupCommon, Name = s_IndeterminateStateName)]
    public class ProgressBar : RangeBase
    {
        static ProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
            PaddingProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(OnPaddingChanged));
            BackgroundProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata { CoerceValueCallback = CoerceBrush });
            ForegroundProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata { CoerceValueCallback = CoerceBrush });
        }

        /// <summary>
        /// Initializes a new instance of the ProgressBar class.
        /// </summary>
        public ProgressBar()
        {
            SizeChanged += OnSizeChanged;
            SetValue(TemplateSettingsPropertyKey, new ProgressBarTemplateSettings());
        }

        #region IsIndeterminate

        /// <summary>
        /// Identifies the IsIndeterminate dependency property.
        /// </summary>
        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register(
                nameof(IsIndeterminate),
                typeof(bool),
                typeof(ProgressBar),
                new PropertyMetadata(false, OnIsIndeterminatePropertyChanged));

        /// <summary>
        /// Gets or sets a value that indicates whether the progress bar reports generic
        /// progress with a repeating pattern or reports progress based on the Value property.
        /// </summary>
        /// <returns>
        /// **True** if the progress bar reports generic progress with a repeating pattern;
        /// **false** if the progress bar reports progress based on the Value property. The
        /// default is **false**.
        /// </returns>
        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        private static void OnIsIndeterminatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ProgressBar)sender).OnIsIndeterminatePropertyChanged(args);
        }

        #endregion

        #region ShowError

        /// <summary>
        /// Identifies the ShowError dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowErrorProperty =
            DependencyProperty.Register(
                nameof(ShowError),
                typeof(bool),
                typeof(ProgressBar),
                new PropertyMetadata(OnShowErrorPropertyChanged));

        /// <summary>
        /// Gets or sets a value that indicates whether the progress bar should use visual
        /// states that communicate an **Error** state to the user.
        /// </summary>
        /// <returns>
        /// **True** if the progress bar should use visual states that communicate an **Error**
        /// state to the user; otherwise, **false**. The default is **false**.
        /// </returns>
        public bool ShowError
        {
            get => (bool)GetValue(ShowErrorProperty);
            set => SetValue(ShowErrorProperty, value);
        }

        private static void OnShowErrorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ProgressBar)sender).OnShowErrorPropertyChanged(args);
        }

        #endregion

        #region ShowPaused

        /// <summary>
        /// Identifies the ShowPaused dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowPausedProperty =
            DependencyProperty.Register(
                nameof(ShowPaused),
                typeof(bool),
                typeof(ProgressBar),
                new PropertyMetadata(OnShowPausedPropertyChanged));

        /// <summary>
        /// Gets or sets a value that indicates whether the progress bar should use visual
        /// states that communicate a **Paused** state to the user.
        /// </summary>
        /// <returns>
        /// **True** if the progress bar should use visual states that communicate a **Paused**
        /// state to the user; otherwise, **false**. The default is **false**.
        /// </returns>
        public bool ShowPaused
        {
            get => (bool)GetValue(ShowPausedProperty);
            set => SetValue(ShowPausedProperty, value);
        }

        private static void OnShowPausedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ProgressBar)sender).OnShowPausedPropertyChanged(args);
        }

        #endregion

        #region TemplateSettings

        private static readonly DependencyPropertyKey TemplateSettingsPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(TemplateSettings),
                typeof(ProgressBarTemplateSettings),
                typeof(ProgressBar),
                null);

        /// <summary>
        /// Identifies the TemplateSettings dependency property.
        /// </summary>
        public static readonly DependencyProperty TemplateSettingsProperty =
            TemplateSettingsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets an object that provides calculated values that can be referenced as **TemplateBinding**
        /// sources when defining templates for a ProgressBar control.
        /// </summary>
        /// <returns>An object that provides calculated values for templates.</returns>
        public ProgressBarTemplateSettings TemplateSettings
        {
            get => (ProgressBarTemplateSettings)GetValue(TemplateSettingsProperty);
            private set => SetValue(TemplateSettingsPropertyKey, value);
        }

        #endregion

        #region CornerRadius

        /// <summary>
        /// Identifies the CornerRadius dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            BorderElement.CornerRadiusProperty.AddOwner(typeof(ProgressBar));

        /// <summary>
        /// Gets or sets the radius for the corners of the control's border.
        /// </summary>
        /// <returns>
        /// The degree to which the corners are rounded, expressed as values of the CornerRadius
        /// structure.
        /// </returns>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_layoutRoot = GetTemplateChild(s_LayoutRootName) as Grid;
            m_determinateProgressBarIndicator = GetTemplateChild(s_DeterminateProgressBarIndicatorName) as Rectangle;
            m_indeterminateProgressBarIndicator = GetTemplateChild(s_IndeterminateProgressBarIndicatorName) as Rectangle;
            m_indeterminateProgressBarIndicator2 = GetTemplateChild(s_IndeterminateProgressBarIndicator2Name) as Rectangle;

            UpdateStates();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetProgressBarIndicatorWidth();
            UpdateWidthBasedTemplateSettings();
            ReapplyIndeterminateStoryboard();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            OnIndicatorWidthComponentChanged();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            OnIndicatorWidthComponentChanged();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            OnIndicatorWidthComponentChanged();
        }

        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressBar)d).OnIndicatorWidthComponentChanged();
        }

        private void OnIndicatorWidthComponentChanged()
        {
            SetProgressBarIndicatorWidth();
        }

        private void OnIsIndeterminatePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            SetProgressBarIndicatorWidth();
            UpdateStates();
            ReapplyIndeterminateStoryboard();
        }

        private void OnShowPausedPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            UpdateStates();
        }

        private void OnShowErrorPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            UpdateStates();
        }

        private void UpdateStates(bool useTransitions = true)
        {
            if (ShowError && IsIndeterminate)
            {
                VisualStateManager.GoToState(this, s_IndeterminateErrorStateName, useTransitions);
            }
            else if (ShowError)
            {
                VisualStateManager.GoToState(this, s_ErrorStateName, useTransitions);
            }
            else if (ShowPaused && IsIndeterminate)
            {
                VisualStateManager.GoToState(this, s_IndeterminatePausedStateName, useTransitions);
            }
            else if (ShowPaused)
            {
                VisualStateManager.GoToState(this, s_PausedStateName, useTransitions);
            }
            else if (IsIndeterminate)
            {
                UpdateWidthBasedTemplateSettings();
                VisualStateManager.GoToState(this, s_IndeterminateStateName, useTransitions);
            }
            else if (!IsIndeterminate)
            {
                VisualStateManager.GoToState(this, s_DeterminateStateName, useTransitions);
            }
        }

        private void SetProgressBarIndicatorWidth()
        {
            var templateSettings = TemplateSettings;

            var progressBar = m_layoutRoot;
            if (progressBar != null)
            {
                var determinateProgressBarIndicator = m_determinateProgressBarIndicator;
                if (determinateProgressBarIndicator != null)
                {
                    double progressBarWidth = progressBar.ActualWidth;
                    double prevIndicatorWidth = determinateProgressBarIndicator.ActualWidth;
                    double maximum = Maximum;
                    double minimum = Minimum;
                    var padding = Padding;

                    // Adds "Updating" state in between to trigger RepositionThemeAnimation Visual Transition
                    // in ProgressBar.xaml when reverting back to previous state
                    VisualStateManager.GoToState(this, s_UpdatingStateName, true);

                    if (IsIndeterminate)
                    {
                        m_determinateProgressBarIndicator.Width = 0;

                        if (m_indeterminateProgressBarIndicator != null)
                        {
                            m_indeterminateProgressBarIndicator.Width = progressBarWidth * 0.4; // 40% of ProgressBar Width
                        }

                        if (m_indeterminateProgressBarIndicator2 != null)
                        {
                            m_indeterminateProgressBarIndicator2.Width = progressBarWidth * 0.6; // 60% of ProgressBar Width
                        }
                    }
                    else if (Math.Abs(maximum - minimum) > double.Epsilon)
                    {
                        double maxIndicatorWidth = progressBarWidth - (padding.Left + padding.Right);
                        double increment = maxIndicatorWidth / (maximum - minimum);
                        double indicatorWidth = increment * (Value - minimum);
                        double widthDelta = indicatorWidth - prevIndicatorWidth;
                        templateSettings.IndicatorLengthDelta = -widthDelta;
                        m_determinateProgressBarIndicator.Width = indicatorWidth;
                    }
                    else
                    {
                        m_determinateProgressBarIndicator.Width = 0; // Error
                    }

                    UpdateStates(); // Reverts back to previous state
                }
            }
        }

        private void UpdateWidthBasedTemplateSettings()
        {
            var templateSettings = TemplateSettings;

            double width;
            double height;
            {
                var progressBar = m_layoutRoot;
                if (progressBar != null)
                {
                    width = m_layoutRoot.ActualWidth;
                    height = m_layoutRoot.ActualHeight;
                }
                else
                {
                    width = 0;
                    height = 0;
                }
            }

            double indeterminateProgressBarIndicatorWidth = width * 0.4; // Indicator width at 40% of ProgressBar
            double indeterminateProgressBarIndicatorWidth2 = width * 0.6; // Indicator width at 60% of ProgressBar

            templateSettings.ContainerAnimationStartPosition = indeterminateProgressBarIndicatorWidth * -1.0; // Position at -100%
            templateSettings.ContainerAnimationEndPosition = indeterminateProgressBarIndicatorWidth * 3.0; // Position at 300%

            templateSettings.Container2AnimationStartPosition = indeterminateProgressBarIndicatorWidth2 * -1.5; // Position at -150%
            templateSettings.Container2AnimationEndPosition = indeterminateProgressBarIndicatorWidth2 * 1.66; // Position at 166%

            templateSettings.ContainerAnimationMidPosition = width * 0.2;

            var padding = Padding;
            var rectangle = new RectangleGeometry(
                new Rect(
                    padding.Left,
                    padding.Top,
                    width - (padding.Right + padding.Left),
                    height - (padding.Bottom + padding.Top)
                    ));

            if (m_indeterminateProgressBarIndicator != null)
            {
                rectangle.RadiusX = m_indeterminateProgressBarIndicator.RadiusX;
                rectangle.RadiusY = m_indeterminateProgressBarIndicator.RadiusY;
            }

            templateSettings.ClipRect = rectangle;

            // TemplateSetting properties from WUXC for backwards compatibility.
            templateSettings.EllipseAnimationEndPosition = (1.0 / 3.0) * width;
            templateSettings.EllipseAnimationWellPosition = (2.0 / 3.0) * width;

            if (width <= 180.0)
            {
                // Small ellipse diameter and offset.
                templateSettings.EllipseDiameter = 4.0;
                templateSettings.EllipseOffset = 4.0;
            }
            else if (width <= 280.0)
            {
                // Medium ellipse diameter and offset.
                templateSettings.EllipseDiameter = 5.0;
                templateSettings.EllipseOffset = 7.0;
            }
            else
            {
                // Large ellipse diameter and offset.
                templateSettings.EllipseDiameter = 6.0;
                templateSettings.EllipseOffset = 9.0;
            }
        }

        private static object CoerceBrush(DependencyObject d, object baseValue)
        {
            if (baseValue is Brush brush && !brush.IsFrozen)
            {
                var clone = brush.CloneCurrentValue();
                clone.Freeze();
                return clone;
            }
            return baseValue;
        }

        private void RefreshStates()
        {
            VisualStateManager.GoToState(this, s_UpdatingStateName, false);
            UpdateStates(false);
        }

        private void ReapplyIndeterminateStoryboard()
        {
            Dispatcher.BeginInvoke(
#if (NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
                (Action) (
#endif
                () => {
                if (IsIndeterminate)
                {
                    RefreshStates();
                }
            }
#if (NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
)
#endif
                , DispatcherPriority.Render);
        }

        Grid m_layoutRoot;
        Rectangle m_determinateProgressBarIndicator;
        Rectangle m_indeterminateProgressBarIndicator;
        Rectangle m_indeterminateProgressBarIndicator2;

        const string s_LayoutRootName = "LayoutRoot";
        const string s_DeterminateProgressBarIndicatorName = "DeterminateProgressBarIndicator";
        const string s_IndeterminateProgressBarIndicatorName = "IndeterminateProgressBarIndicator";
        const string s_IndeterminateProgressBarIndicator2Name = "IndeterminateProgressBarIndicator2";
        const string s_ErrorStateName = "Error";
        const string s_PausedStateName = "Paused";
        const string s_IndeterminateStateName = "Indeterminate";
        const string s_IndeterminateErrorStateName = "IndeterminateError";
        const string s_IndeterminatePausedStateName = "IndeterminatePaused";
        const string s_DeterminateStateName = "Determinate";
        const string s_UpdatingStateName = "Updating";
        const string GroupCommon = "CommonStates";
    }
}
