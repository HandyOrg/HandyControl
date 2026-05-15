using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace HandyControl.Controls
{
    public class Window : Avalonia.Controls.Window
    {
        private const string ElementNonClientArea = "PART_NonClientArea";
        private Grid _nonClientArea;
        public Window()
        {

        }

        #region Styled properties

        public static readonly StyledProperty<object> NonClientAreaContentProperty =
            AvaloniaProperty.Register<Window, object>(nameof(NonClientAreaContent), default);

        public object NonClientAreaContent
        {
            get => GetValue(NonClientAreaContentProperty);
            set => SetValue(NonClientAreaContentProperty, value);
        }

        public static readonly StyledProperty<IBrush> CloseButtonHoverBackgroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(CloseButtonHoverBackground), null);

        public IBrush CloseButtonHoverBackground
        {
            get => GetValue(CloseButtonHoverBackgroundProperty);
            set => SetValue(CloseButtonHoverBackgroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> CloseButtonHoverForegroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(CloseButtonHoverForeground), null);

        public IBrush CloseButtonHoverForeground
        {
            get => GetValue(CloseButtonHoverForegroundProperty);
            set => SetValue(CloseButtonHoverForegroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> CloseButtonBackgroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(CloseButtonBackground), Brushes.Transparent);

        public IBrush CloseButtonBackground
        {
            get => GetValue(CloseButtonBackgroundProperty);
            set => SetValue(CloseButtonBackgroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> CloseButtonForegroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(CloseButtonForeground), Brushes.White);

        public IBrush CloseButtonForeground
        {
            get => GetValue(CloseButtonForegroundProperty);
            set => SetValue(CloseButtonForegroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> OtherButtonBackgroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(OtherButtonBackground), Brushes.Transparent);

        public IBrush OtherButtonBackground
        {
            get => GetValue(OtherButtonBackgroundProperty);
            set => SetValue(OtherButtonBackgroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> OtherButtonForegroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(OtherButtonForeground), Brushes.White);

        public IBrush OtherButtonForeground
        {
            get => GetValue(OtherButtonForegroundProperty);
            set => SetValue(OtherButtonForegroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> OtherButtonHoverBackgroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(OtherButtonHoverBackground), null);

        public IBrush OtherButtonHoverBackground
        {
            get => GetValue(OtherButtonHoverBackgroundProperty);
            set => SetValue(OtherButtonHoverBackgroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> OtherButtonHoverForegroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(OtherButtonHoverForeground), null);

        public IBrush OtherButtonHoverForeground
        {
            get => GetValue(OtherButtonHoverForegroundProperty);
            set => SetValue(OtherButtonHoverForegroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> NonClientAreaBackgroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(NonClientAreaBackground), null);

        public IBrush NonClientAreaBackground
        {
            get => GetValue(NonClientAreaBackgroundProperty);
            set => SetValue(NonClientAreaBackgroundProperty, value);
        }

        public static readonly StyledProperty<IBrush> NonClientAreaForegroundProperty =
            AvaloniaProperty.Register<Window, IBrush>(nameof(NonClientAreaForeground), null);

        public IBrush NonClientAreaForeground
        {
            get => GetValue(NonClientAreaForegroundProperty);
            set => SetValue(NonClientAreaForegroundProperty, value);
        }

        public static readonly StyledProperty<double> NonClientAreaHeightProperty =
            AvaloniaProperty.Register<Window, double>(nameof(NonClientAreaHeight), 22.0);

        public double NonClientAreaHeight
        {
            get => GetValue(NonClientAreaHeightProperty);
            set => SetValue(NonClientAreaHeightProperty, value);
        }

        public static readonly StyledProperty<bool> ShowNonClientAreaProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(ShowNonClientArea), true);

        public bool ShowNonClientArea
        {
            get => GetValue(ShowNonClientAreaProperty);
            set => SetValue(ShowNonClientAreaProperty, value);
        }

        public static readonly StyledProperty<bool> ShowTitleProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(ShowTitle), true);

        public bool ShowTitle
        {
            get => GetValue(ShowTitleProperty);
            set => SetValue(ShowTitleProperty, value);
        }

        public static readonly StyledProperty<bool> IsFullScreenProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(IsFullScreen), false);

        public bool IsFullScreen
        {
            get => GetValue(IsFullScreenProperty);
            set => SetValue(IsFullScreenProperty, value);
        }

        public static readonly StyledProperty<bool> ShowIconProperty =
            AvaloniaProperty.Register<Window, bool>(nameof(ShowIcon), true);

        public bool ShowIcon
        {
            get => GetValue(ShowIconProperty);
            set => SetValue(ShowIconProperty, value);
        }

        #endregion


        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            _nonClientArea = e.NameScope.Find<Grid>(ElementNonClientArea);
            if (_nonClientArea != null)
            {
                _nonClientArea.PointerPressed += (_, e) =>
                {

                    if (e.Pointer.Type == PointerType.Mouse && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                    {
                        this.BeginMoveDrag(e);
                    }
                };
            }
            base.OnApplyTemplate(e);
        }
    }
}
