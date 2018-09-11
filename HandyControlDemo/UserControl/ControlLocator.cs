using System;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace HandyControlDemo.UserControl
{
	/// <summary>
    ///     控件定位器
    /// </summary>
    public class ControlLocator
	{
		private ControlLocator()
        {
			SimpleIoc.Default.Register<MainContent>();
			SimpleIoc.Default.Register<LeftMainContent>();
			SimpleIoc.Default.Register<NoUserContent>();
			SimpleIoc.Default.Register<GrowlDemoCtl>();
			SimpleIoc.Default.Register<LoadingDemoCtl>();
			SimpleIoc.Default.Register<ImageBrowserDemoCtl>();
			SimpleIoc.Default.Register<ColorPickerDemoCtl>();
			SimpleIoc.Default.Register<CarouselDemoCtl>();
			SimpleIoc.Default.Register<CompareSliderDemoCtl>();
			SimpleIoc.Default.Register<TimeBarDemoCtl>();
			SimpleIoc.Default.Register<PaginationDemoCtl>();
			SimpleIoc.Default.Register<AnimationPathDemoCtl>();
			SimpleIoc.Default.Register<ButtonDemoCtl>();
			SimpleIoc.Default.Register<ToggleButtonDemoCtl>();
			SimpleIoc.Default.Register<ExpanderDemoCtl>();
			SimpleIoc.Default.Register<ProgressBarDemoCtl>();
			SimpleIoc.Default.Register<TabControlDemoCtl>();
		}

		public static ControlLocator Instance => new Lazy<ControlLocator>(() => new ControlLocator()).Value;

		#region Main

		public MainContent MainContent => ServiceLocator.Current.GetInstance<MainContent>();

		public LeftMainContent LeftMainContent => ServiceLocator.Current.GetInstance<LeftMainContent>();

		public NoUserContent NoUserContent => ServiceLocator.Current.GetInstance<NoUserContent>();

		#endregion

		#region Controls

		public GrowlDemoCtl GrowlDemoCtl => new GrowlDemoCtl();

		public LoadingDemoCtl LoadingDemoCtl => new LoadingDemoCtl();

		public ImageBrowserDemoCtl ImageBrowserDemoCtl => new ImageBrowserDemoCtl();

		public ColorPickerDemoCtl ColorPickerDemoCtl => new ColorPickerDemoCtl();

		public CarouselDemoCtl CarouselDemoCtl => new CarouselDemoCtl();

		public CompareSliderDemoCtl CompareSliderDemoCtl => new CompareSliderDemoCtl();

		public TimeBarDemoCtl TimeBarDemoCtl => new TimeBarDemoCtl();

		public PaginationDemoCtl PaginationDemoCtl => new PaginationDemoCtl();

		public AnimationPathDemoCtl AnimationPathDemoCtl => new AnimationPathDemoCtl();

		#endregion

		#region Styles

		public ButtonDemoCtl ButtonDemoCtl => new ButtonDemoCtl();

		public ToggleButtonDemoCtl ToggleButtonDemoCtl => new ToggleButtonDemoCtl();

		public ExpanderDemoCtl ExpanderDemoCtl => new ExpanderDemoCtl();

		public ProgressBarDemoCtl ProgressBarDemoCtl => new ProgressBarDemoCtl();

		public TabControlDemoCtl TabControlDemoCtl => new TabControlDemoCtl();

		#endregion
	}
}