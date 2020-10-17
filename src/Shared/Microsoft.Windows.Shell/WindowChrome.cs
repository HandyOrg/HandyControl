using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Data;
using Standard;

namespace Microsoft.Windows.Shell
{
	// Token: 0x020000A4 RID: 164
	public class WindowChrome : Freezable
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00008A43 File Offset: 0x00006C43
		public static Thickness GlassFrameCompleteThickness
		{
			get
			{
				return new Thickness(-1.0);
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00008A54 File Offset: 0x00006C54
		private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				return;
			}
			Window window = (Window)d;
			WindowChrome windowChrome = (WindowChrome)e.NewValue;
			WindowChromeWorker windowChromeWorker = WindowChromeWorker.GetWindowChromeWorker(window);
			if (windowChromeWorker == null)
			{
				windowChromeWorker = new WindowChromeWorker();
				WindowChromeWorker.SetWindowChromeWorker(window, windowChromeWorker);
			}
			windowChromeWorker.SetWindowChrome(windowChrome);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00008A9C File Offset: 0x00006C9C
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public static WindowChrome GetWindowChrome(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChrome)window.GetValue(WindowChrome.WindowChromeProperty);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00008AB9 File Offset: 0x00006CB9
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public static void SetWindowChrome(Window window, WindowChrome chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChrome.WindowChromeProperty, chrome);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00008AD4 File Offset: 0x00006CD4
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
			return (bool)dependencyObject.GetValue(WindowChrome.IsHitTestVisibleInChromeProperty);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00008B18 File Offset: 0x00006D18
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00008B5B File Offset: 0x00006D5B
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00008B6D File Offset: 0x00006D6D
		public double CaptionHeight
		{
			get
			{
				return (double)base.GetValue(WindowChrome.CaptionHeightProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CaptionHeightProperty, value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00008B80 File Offset: 0x00006D80
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00008B92 File Offset: 0x00006D92
		public Thickness ResizeBorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.ResizeBorderThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.ResizeBorderThicknessProperty, value);
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00008BA5 File Offset: 0x00006DA5
		private static object _CoerceGlassFrameThickness(Thickness thickness)
		{
			if (!Utility.IsThicknessNonNegative(thickness))
			{
				return WindowChrome.GlassFrameCompleteThickness;
			}
			return thickness;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600031A RID: 794 RVA: 0x00008BC0 File Offset: 0x00006DC0
		// (set) Token: 0x0600031B RID: 795 RVA: 0x00008BD2 File Offset: 0x00006DD2
		public Thickness GlassFrameThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.GlassFrameThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.GlassFrameThicknessProperty, value);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00008BE5 File Offset: 0x00006DE5
		// (set) Token: 0x0600031D RID: 797 RVA: 0x00008BF7 File Offset: 0x00006DF7
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(WindowChrome.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CornerRadiusProperty, value);
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00008C0A File Offset: 0x00006E0A
		protected override Freezable CreateInstanceCore()
		{
			return new WindowChrome();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00008C14 File Offset: 0x00006E14
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

		// Token: 0x06000320 RID: 800 RVA: 0x00008CAC File Offset: 0x00006EAC
		private void _OnPropertyChangedThatRequiresRepaint()
		{
			EventHandler propertyChangedThatRequiresRepaint = this.PropertyChangedThatRequiresRepaint;
			if (propertyChangedThatRequiresRepaint != null)
			{
				propertyChangedThatRequiresRepaint(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000321 RID: 801 RVA: 0x00008CD0 File Offset: 0x00006ED0
		// (remove) Token: 0x06000322 RID: 802 RVA: 0x00008D08 File Offset: 0x00006F08
		internal event EventHandler PropertyChangedThatRequiresRepaint;

		// Token: 0x040005EA RID: 1514
		public static readonly DependencyProperty WindowChromeProperty = DependencyProperty.RegisterAttached("WindowChrome", typeof(WindowChrome), typeof(WindowChrome), new PropertyMetadata(null, new PropertyChangedCallback(WindowChrome._OnChromeChanged)));

		// Token: 0x040005EB RID: 1515
		public static readonly DependencyProperty IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040005EC RID: 1516
		public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowChrome), new PropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => (double)value >= 0.0);

		// Token: 0x040005ED RID: 1517
		public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness)), (object value) => Utility.IsThicknessNonNegative((Thickness)value));

		// Token: 0x040005EE RID: 1518
		public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}, (DependencyObject d, object o) => WindowChrome._CoerceGlassFrameThickness((Thickness)o)));

		// Token: 0x040005EF RID: 1519
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowChrome), new PropertyMetadata(default(CornerRadius), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => Utility.IsCornerRadiusValid((CornerRadius)value));

		// Token: 0x040005F0 RID: 1520
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

		// Token: 0x020000A5 RID: 165
		private struct _SystemParameterBoundProperty
		{
			// Token: 0x17000059 RID: 89
			// (get) Token: 0x0600032B RID: 811 RVA: 0x00009065 File Offset: 0x00007265
			// (set) Token: 0x0600032C RID: 812 RVA: 0x0000906D File Offset: 0x0000726D
			public string SystemParameterPropertyName { get; set; }

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x0600032D RID: 813 RVA: 0x00009076 File Offset: 0x00007276
			// (set) Token: 0x0600032E RID: 814 RVA: 0x0000907E File Offset: 0x0000727E
			public DependencyProperty DependencyProperty { get; set; }
		}
	}
}
