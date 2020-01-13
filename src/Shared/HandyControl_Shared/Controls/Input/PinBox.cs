using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls
{
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    public class PinBox : Control
    {
        private const string ElementPanel = "PART_Panel";

        private static readonly object MinLength = 4;

        private Panel _panel;

        private int _inputIndex;

        private bool _changed;

        private RoutedEventHandler _passwordBoxsGotFocusEventHandler;

        private RoutedEventHandler _passwordBoxsPasswordChangedEventHandler;

        public PinBox()
        {
            Loaded += PinBox_Loaded;
            Unloaded += PinBox_Unloaded;
        }

        private void PinBox_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, _passwordBoxsPasswordChangedEventHandler);
            RemoveHandler(GotFocusEvent, _passwordBoxsGotFocusEventHandler);
            
            Loaded -= PinBox_Loaded;
            Unloaded -= PinBox_Unloaded;
        }

        private void PinBox_Loaded(object sender, RoutedEventArgs e)
        {
            _passwordBoxsPasswordChangedEventHandler = PasswordBoxsPasswordChanged;
            AddHandler(System.Windows.Controls.PasswordBox.PasswordChangedEvent, _passwordBoxsPasswordChangedEventHandler);
            
            _passwordBoxsGotFocusEventHandler = PasswordBoxsGotFocus;
            AddHandler(GotFocusEvent, _passwordBoxsGotFocusEventHandler);
        }

        private void PasswordBoxsPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.PasswordBox passwordBox)
            {
                if (passwordBox.Password.Length > 0)
                {
                    if (++_inputIndex >= Length)
                    {
                        _inputIndex = Length - 1;

                        if (_panel.Children.OfType<System.Windows.Controls.PasswordBox>()
                            .All(item => item.Password.Any()))
                        {
                            FocusManager.SetFocusedElement(this, null);
                            Keyboard.ClearFocus();
                            RaiseEvent(new RoutedEventArgs(CompletedEvent, this));
                        }
                        return;
                    }
                }
                else
                {
                    if (--_inputIndex < 0)
                    {
                        _inputIndex = 0;
                        return;
                    }
                }

                _changed = true;
                FocusManager.SetFocusedElement(this, _panel.Children[_inputIndex]);
            }
        }

        private void PasswordBoxsGotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.PasswordBox passwordBox)
            {
                _inputIndex = _panel.Children.IndexOf(passwordBox);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);

            if (e.Key == Key.Left)
            {
                if (--_inputIndex < 0)
                {
                    _inputIndex = 0;
                    return;
                }

                FocusManager.SetFocusedElement(this, _panel.Children[_inputIndex]);
            }
            else if (e.Key == Key.Right)
            {
                if (++_inputIndex >= Length)
                {
                    _inputIndex = Length - 1;
                    return;
                }

                FocusManager.SetFocusedElement(this, _panel.Children[_inputIndex]);
            }
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);

            if (_changed)
            {
                _changed = false;
                return;
            }

            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (--_inputIndex < 0)
                {
                    _inputIndex = 0;
                    return;
                }

                FocusManager.SetFocusedElement(this, _panel.Children[_inputIndex]);
            }
        }

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
            "Length", typeof(int), typeof(PinBox), new PropertyMetadata(MinLength, null, CoerceLength), ValidateHelper.IsInRangeOfPosInt);

        private static object CoerceLength(DependencyObject d, object basevalue) => (int)basevalue < 4 ? MinLength : basevalue;

        public int Length
        {
            get => (int) GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            "ItemMargin", typeof(Thickness), typeof(PinBox), new PropertyMetadata(default(Thickness)));

        public Thickness ItemMargin
        {
            get => (Thickness) GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
            "ItemWidth", typeof(double), typeof(PinBox), new PropertyMetadata(ValueBoxes.Double0Box));

        public double ItemWidth
        {
            get => (double) GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
            "ItemHeight", typeof(double), typeof(PinBox), new PropertyMetadata(ValueBoxes.Double0Box));

        public double ItemHeight
        {
            get => (double) GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public static readonly RoutedEvent CompletedEvent =
            EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(PinBox));

        public event EventHandler Completed
        {
            add => AddHandler(CompletedEvent, value);
            remove => RemoveHandler(CompletedEvent, value);
        }

        private void UpdateItems()
        {
            _panel.Children.Clear();
            var length = Length;

            for (var i = 0; i < length; i++)
            {
                _panel.Children.Add(CreatePasswordBox());
            }
        }

        private System.Windows.Controls.PasswordBox CreatePasswordBox()
        {
            return new System.Windows.Controls.PasswordBox
            {
                MaxLength = 1,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = ItemMargin,
                Width = ItemWidth,
                Height = ItemHeight,
                Padding = new Thickness()
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _panel = GetTemplateChild(ElementPanel) as Panel;
            if (_panel != null)
            {
                UpdateItems();
            }
        }
    }
}
