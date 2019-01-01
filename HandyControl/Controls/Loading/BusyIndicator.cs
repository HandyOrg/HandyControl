using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HandyControl.Controls
{
    /// <summary>
    /// A control to provide a visual indicator when an application is busy.
    /// </summary>
    [TemplateVisualState(Name = VisualStates.StateIdle, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateBusy, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof(Rectangle))]
    [StyleTypedProperty(Property = "ProgressBarStyle", StyleTargetType = typeof(ProgressBar))]
    public class BusyIndicator : ContentControl
    {
        #region Private Members

        /// <summary>
        /// Timer used to delay the initial display and avoid flickering.
        /// </summary>
        private DispatcherTimer _displayAfterTimer = new DispatcherTimer();

        #endregion //Private Members

        #region Constructors

        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public BusyIndicator()
        {
            _displayAfterTimer.Tick += DisplayAfterTimerElapsed;
        }

        #endregion //Constructors

        #region Base Class Overrides

        /// <summary>
        /// Overrides the OnApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }


        #endregion //Base Class Overrides

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the BusyContent is visible.
        /// </summary>
        protected bool IsContentVisible
        {
            get;
            set;
        }

        #endregion //Properties

        #region Dependency Properties

        #region IsBusy

        /// <summary>
        /// Identifies the IsBusy dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether the busy indicator should show.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="d">BusyIndicator that changed its IsBusy.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Go visible now
                    IsContentVisible = true;
                }
                else
                {
                    // Set a timer to go visible
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // No longer visible
                _displayAfterTimer.Stop();
                IsContentVisible = false;

                if (this.FocusAfterBusy != null)
                {
                    this.FocusAfterBusy.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                    {
                        this.FocusAfterBusy.Focus();
                    }
                    ));
                }
            }

            ChangeVisualState(true);
        }

        #endregion //IsBusy

        #region Busy Content

        /// <summary>
        /// Identifies the BusyContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating the busy content to display to the user.
        /// </summary>
        public object BusyContent
        {
            get
            {
                return (object)GetValue(BusyContentProperty);
            }
            set
            {
                SetValue(BusyContentProperty, value);
            }
        }

        #endregion //Busy Content

        #region Busy Content Template

        /// <summary>
        /// Identifies the BusyTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating the template to use for displaying the busy content to the user.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(BusyContentTemplateProperty);
            }
            set
            {
                SetValue(BusyContentTemplateProperty, value);
            }
        }

        #endregion //Busy Content Template

        #region Display After

        /// <summary>
        /// Identifies the DisplayAfter dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// Gets or sets a value indicating how long to delay before displaying the busy content.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get
            {
                return (TimeSpan)GetValue(DisplayAfterProperty);
            }
            set
            {
                SetValue(DisplayAfterProperty, value);
            }
        }

        #endregion //Display After

        #region FocusAfterBusy

        /// <summary>
        /// Identifies the FocusAfterBusy dependency property.
        /// </summary>
        public static readonly DependencyProperty FocusAfterBusyProperty = DependencyProperty.Register(
            "FocusAfterBusy",
            typeof(Control),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a Control that should get the focus when the busy indicator disapears.
        /// </summary>
        public Control FocusAfterBusy
        {
            get
            {
                return (Control)GetValue(FocusAfterBusyProperty);
            }
            set
            {
                SetValue(FocusAfterBusyProperty, value);
            }
        }

        #endregion //FocusAfterBusy

        #region Overlay Style

        /// <summary>
        /// Identifies the OverlayStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating the style to use for the overlay.
        /// </summary>
        public Style OverlayStyle
        {
            get
            {
                return (Style)GetValue(OverlayStyleProperty);
            }
            set
            {
                SetValue(OverlayStyleProperty, value);
            }
        }
        #endregion //Overlay Style

        #region ProgressBar Style

        /// <summary>
        /// Identifies the ProgressBarStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating the style to use for the progress bar.
        /// </summary>
        public Style ProgressBarStyle
        {
            get
            {
                return (Style)GetValue(ProgressBarStyleProperty);
            }
            set
            {
                SetValue(ProgressBarStyleProperty, value);
            }
        }

        #endregion //ProgressBar Style

        #endregion //Dependency Properties

        #region Methods

        /// <summary>
        /// Handler for the DisplayAfterTimer.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions">True if state transitions should be used.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        #endregion //Methods
    }
}
