using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Data;
using Standard;

namespace Microsoft.Windows.Shell;

public class WindowChrome : Freezable
{
    public static Thickness GlassFrameCompleteThickness
    {
        get
        {
            return new Thickness(-1.0);
        }
    }

    private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(d))
        {
            return;
        }
        Window window = (Window) d;
        WindowChrome windowChrome = (WindowChrome) e.NewValue;
        WindowChromeWorker windowChromeWorker = WindowChromeWorker.GetWindowChromeWorker(window);
        if (windowChromeWorker == null)
        {
            windowChromeWorker = new WindowChromeWorker();
            WindowChromeWorker.SetWindowChromeWorker(window, windowChromeWorker);
        }
        windowChromeWorker.SetWindowChrome(windowChrome);
    }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
    public static WindowChrome GetWindowChrome(Window window)
    {
        Verify.IsNotNull<Window>(window, "window");
        return (WindowChrome) window.GetValue(WindowChrome.WindowChromeProperty);
    }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
    public static void SetWindowChrome(Window window, WindowChrome chrome)
    {
        Verify.IsNotNull<Window>(window, "window");
        window.SetValue(WindowChrome.WindowChromeProperty, chrome);
    }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
    public static bool GetIsHitTestVisibleInChrome(IInputElement inputElement)
    {
        Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
        DependencyObject dependencyObject = inputElement as DependencyObject;
        if (dependencyObject == null)
        {
            throw new ArgumentException("The element must be a DependencyObject", "inputElement");
        }
        return (bool) dependencyObject.GetValue(WindowChrome.IsHitTestVisibleInChromeProperty);
    }

    [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
    [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
    public static void SetIsHitTestVisibleInChrome(IInputElement inputElement, bool hitTestVisible)
    {
        Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
        DependencyObject dependencyObject = inputElement as DependencyObject;
        if (dependencyObject == null)
        {
            throw new ArgumentException("The element must be a DependencyObject", "inputElement");
        }
        dependencyObject.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, hitTestVisible);
    }

    public double CaptionHeight
    {
        get
        {
            return (double) base.GetValue(WindowChrome.CaptionHeightProperty);
        }
        set
        {
            base.SetValue(WindowChrome.CaptionHeightProperty, value);
        }
    }

    public Thickness ResizeBorderThickness
    {
        get
        {
            return (Thickness) base.GetValue(WindowChrome.ResizeBorderThicknessProperty);
        }
        set
        {
            base.SetValue(WindowChrome.ResizeBorderThicknessProperty, value);
        }
    }

    private static object _CoerceGlassFrameThickness(Thickness thickness)
    {
        if (!Utility.IsThicknessNonNegative(thickness))
        {
            return WindowChrome.GlassFrameCompleteThickness;
        }
        return thickness;
    }

    public Thickness GlassFrameThickness
    {
        get
        {
            return (Thickness) base.GetValue(WindowChrome.GlassFrameThicknessProperty);
        }
        set
        {
            base.SetValue(WindowChrome.GlassFrameThicknessProperty, value);
        }
    }

    public CornerRadius CornerRadius
    {
        get
        {
            return (CornerRadius) base.GetValue(WindowChrome.CornerRadiusProperty);
        }
        set
        {
            base.SetValue(WindowChrome.CornerRadiusProperty, value);
        }
    }

    protected override Freezable CreateInstanceCore()
    {
        return new WindowChrome();
    }

    public WindowChrome()
    {
        foreach (WindowChrome._SystemParameterBoundProperty systemParameterBoundProperty in WindowChrome._BoundProperties)
        {
            BindingOperations.SetBinding(this, systemParameterBoundProperty.DependencyProperty, new Binding
            {
                Source = SystemParameters2.Current,
                Path = new PropertyPath(systemParameterBoundProperty.SystemParameterPropertyName, new object[0]),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
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

    internal event EventHandler PropertyChangedThatRequiresRepaint;

    public static readonly DependencyProperty WindowChromeProperty = DependencyProperty.RegisterAttached("WindowChrome", typeof(WindowChrome), typeof(WindowChrome), new PropertyMetadata(null, new PropertyChangedCallback(WindowChrome._OnChromeChanged)));

    public static readonly DependencyProperty IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowChrome), new PropertyMetadata(0.0, delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint();
    }), (object value) => (double) value >= 0.0);

    public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness)), (object value) => Utility.IsThicknessNonNegative((Thickness) value));

    public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint();
    }, (DependencyObject d, object o) => WindowChrome._CoerceGlassFrameThickness((Thickness) o)));

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowChrome), new PropertyMetadata(default(CornerRadius), delegate (DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((WindowChrome) d)._OnPropertyChangedThatRequiresRepaint();
    }), (object value) => Utility.IsCornerRadiusValid((CornerRadius) value));

    private static readonly List<WindowChrome._SystemParameterBoundProperty> _BoundProperties = new List<WindowChrome._SystemParameterBoundProperty>
    {
        new WindowChrome._SystemParameterBoundProperty
        {
            DependencyProperty = WindowChrome.CornerRadiusProperty,
            SystemParameterPropertyName = "WindowCornerRadius"
        },
        new WindowChrome._SystemParameterBoundProperty
        {
            DependencyProperty = WindowChrome.CaptionHeightProperty,
            SystemParameterPropertyName = "WindowCaptionHeight"
        },
        new WindowChrome._SystemParameterBoundProperty
        {
            DependencyProperty = WindowChrome.ResizeBorderThicknessProperty,
            SystemParameterPropertyName = "WindowResizeBorderThickness"
        },
        new WindowChrome._SystemParameterBoundProperty
        {
            DependencyProperty = WindowChrome.GlassFrameThicknessProperty,
            SystemParameterPropertyName = "WindowNonClientFrameThickness"
        }
    };

    private struct _SystemParameterBoundProperty
    {
        public string SystemParameterPropertyName { get; set; }

        public DependencyProperty DependencyProperty { get; set; }
    }
}
