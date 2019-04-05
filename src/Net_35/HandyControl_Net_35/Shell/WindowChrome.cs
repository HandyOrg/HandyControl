namespace Microsoft.Windows.Shell
{
    using Standard;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Data;

    public class WindowChrome : Freezable
    {
        private static readonly List<_SystemParameterBoundProperty> _BoundProperties;
        public static readonly DependencyProperty CaptionHeightProperty;
        public static readonly DependencyProperty CornerRadiusProperty;
        public static readonly DependencyProperty GlassFrameThicknessProperty;
        public static readonly DependencyProperty IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
        public static readonly DependencyProperty ResizeBorderThicknessProperty;
        public static readonly DependencyProperty WindowChromeProperty = DependencyProperty.RegisterAttached("WindowChrome", typeof(WindowChrome), typeof(WindowChrome), new PropertyMetadata(null, new PropertyChangedCallback(WindowChrome._OnChromeChanged)));

        internal event EventHandler PropertyChangedThatRequiresRepaint;

        static WindowChrome()
        {
            CaptionHeightProperty = DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowChrome), new PropertyMetadata(0.0, (d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint()), value => ((double) value) >= 0.0);
            ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(new Thickness()), value => Standard.Utility.IsThicknessNonNegative((Thickness) value));
            GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(new Thickness(), (d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint(), (d, o) => _CoerceGlassFrameThickness((Thickness) o)));
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(System.Windows.CornerRadius), typeof(WindowChrome), new PropertyMetadata(new System.Windows.CornerRadius(), (d, e) => ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint()), value => Standard.Utility.IsCornerRadiusValid((System.Windows.CornerRadius) value));
            List<_SystemParameterBoundProperty> list = new List<_SystemParameterBoundProperty>();
            _SystemParameterBoundProperty item = new _SystemParameterBoundProperty {
                DependencyProperty = CornerRadiusProperty,
                SystemParameterPropertyName = "WindowCornerRadius"
            };
            list.Add(item);
            _SystemParameterBoundProperty property2 = new _SystemParameterBoundProperty {
                DependencyProperty = CaptionHeightProperty,
                SystemParameterPropertyName = "WindowCaptionHeight"
            };
            list.Add(property2);
            _SystemParameterBoundProperty property3 = new _SystemParameterBoundProperty {
                DependencyProperty = ResizeBorderThicknessProperty,
                SystemParameterPropertyName = "WindowResizeBorderThickness"
            };
            list.Add(property3);
            _SystemParameterBoundProperty property4 = new _SystemParameterBoundProperty {
                DependencyProperty = GlassFrameThicknessProperty,
                SystemParameterPropertyName = "WindowNonClientFrameThickness"
            };
            list.Add(property4);
            _BoundProperties = list;
        }

        public WindowChrome()
        {
            foreach (_SystemParameterBoundProperty property in _BoundProperties)
            {
                Binding binding = new Binding {
                    Source = SystemParameters2.Current,
                    Path = new PropertyPath(property.SystemParameterPropertyName, new object[0]),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(this, property.DependencyProperty, binding);
            }
        }

        private static object _CoerceGlassFrameThickness(Thickness thickness)
        {
            if (!Standard.Utility.IsThicknessNonNegative(thickness))
            {
                return GlassFrameCompleteThickness;
            }
            return thickness;
        }

        private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(d))
            {
                Window window = (Window) d;
                WindowChrome newValue = (WindowChrome) e.NewValue;
                WindowChromeWorker windowChromeWorker = WindowChromeWorker.GetWindowChromeWorker(window);
                if (windowChromeWorker == null)
                {
                    windowChromeWorker = new WindowChromeWorker();
                    WindowChromeWorker.SetWindowChromeWorker(window, windowChromeWorker);
                }
                windowChromeWorker.SetWindowChrome(newValue);
            }
        }

        private void _OnPropertyChangedThatRequiresRepaint()
        {
            EventHandler propertyChangedThatRequiresRepaint = this.PropertyChangedThatRequiresRepaint;
            if (propertyChangedThatRequiresRepaint != null)
            {
                propertyChangedThatRequiresRepaint(this, EventArgs.Empty);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new WindowChrome();
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0")]
        public static bool GetIsHitTestVisibleInChrome(IInputElement inputElement)
        {
            Standard.Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
            DependencyObject obj2 = inputElement as DependencyObject;
            if (obj2 == null)
            {
                throw new ArgumentException("The element must be a DependencyObject", "inputElement");
            }
            return (bool) obj2.GetValue(IsHitTestVisibleInChromeProperty);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0")]
        public static WindowChrome GetWindowChrome(Window window)
        {
            Standard.Verify.IsNotNull<Window>(window, "window");
            return (WindowChrome) window.GetValue(WindowChromeProperty);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0")]
        public static void SetIsHitTestVisibleInChrome(IInputElement inputElement, bool hitTestVisible)
        {
            Standard.Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
            DependencyObject obj2 = inputElement as DependencyObject;
            if (obj2 == null)
            {
                throw new ArgumentException("The element must be a DependencyObject", "inputElement");
            }
            obj2.SetValue(IsHitTestVisibleInChromeProperty, hitTestVisible);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0")]
        public static void SetWindowChrome(Window window, WindowChrome chrome)
        {
            Standard.Verify.IsNotNull<Window>(window, "window");
            window.SetValue(WindowChromeProperty, chrome);
        }

        public double CaptionHeight
        {
            get
            {
                return (double) base.GetValue(CaptionHeightProperty);
            }
            set
            {
                base.SetValue(CaptionHeightProperty, value);
            }
        }

        public System.Windows.CornerRadius CornerRadius
        {
            get
            {
                return (System.Windows.CornerRadius) base.GetValue(CornerRadiusProperty);
            }
            set
            {
                base.SetValue(CornerRadiusProperty, value);
            }
        }

        public static Thickness GlassFrameCompleteThickness
        {
            get
            {
                return new Thickness(-1.0);
            }
        }

        public Thickness GlassFrameThickness
        {
            get
            {
                return (Thickness) base.GetValue(GlassFrameThicknessProperty);
            }
            set
            {
                base.SetValue(GlassFrameThicknessProperty, value);
            }
        }

        public Thickness ResizeBorderThickness
        {
            get
            {
                return (Thickness) base.GetValue(ResizeBorderThicknessProperty);
            }
            set
            {
                base.SetValue(ResizeBorderThicknessProperty, value);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct _SystemParameterBoundProperty
        {
            public string SystemParameterPropertyName { get; set; }
            public System.Windows.DependencyProperty DependencyProperty { get; set; }
        }
    }
}

