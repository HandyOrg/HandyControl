using HandyControlDemo.ViewModels;
using HandyControlDemo.Views;
using HandyControlDemo.Window;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

namespace HandyControlDemo
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override void InitializeShell(DependencyObject shell)
        {
            base.InitializeShell(shell);

            // Default View
            Container.Resolve<IRegionManager>().RegisterViewWithRegion("ContentRegion", typeof(UnderConstruction));
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region Main
            containerRegistry.RegisterForNavigation<ProjectsView>();
            containerRegistry.RegisterForNavigation<QQGroupView>();
            containerRegistry.RegisterForNavigation<WebsitesView>();
            containerRegistry.RegisterForNavigation<BlogsView>();
            containerRegistry.RegisterForNavigation<ContributorsView>();
            #endregion

            #region Tools
            containerRegistry.RegisterForNavigation<EffectsDemoCtl>();
            containerRegistry.RegisterForNavigation<GeometryAnimationDemoCtl>();
            containerRegistry.RegisterForNavigation<HatchBrushGeneratorDemoCtl>();
            #endregion

            #region Styles
            containerRegistry.RegisterForNavigation<BorderDemoCtl>();
            containerRegistry.RegisterForNavigation<BrushDemoCtl>();
            containerRegistry.RegisterForNavigation<ButtonDemoCtl>();
            containerRegistry.RegisterForNavigation<CalendarDemoCtl>();
            containerRegistry.RegisterForNavigation<CheckBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<DataGridDemoCtl>();
            containerRegistry.RegisterForNavigation<ExpanderDemoCtl>();
            containerRegistry.RegisterForNavigation<FlowDocumentDemoCtl>();
            containerRegistry.RegisterForNavigation<FrameDemoCtl>();
            containerRegistry.RegisterForNavigation<GeometryDemoCtl>();
            containerRegistry.RegisterForNavigation<GroupBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<LabelDemoCtl>();
            containerRegistry.RegisterForNavigation<ListBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<ListViewDemoCtl>();
            containerRegistry.RegisterForNavigation<MenuDemoCtl>();
            containerRegistry.RegisterForNavigation<NativeComboBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<NativeDatePickerDemoCtl>();
            containerRegistry.RegisterForNavigation<NativePasswordBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<NativeProgressBarDemoCtl>();
            containerRegistry.RegisterForNavigation<NativeScrollViewerDemoCtl>();
            containerRegistry.RegisterForNavigation<NativeTabControlDemoCtl>();
            containerRegistry.RegisterForNavigation<NativeTextBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<NativeWindowDemoCtl>();
            containerRegistry.RegisterForNavigation<RadioButtonDemoCtl>();
            containerRegistry.RegisterForNavigation<RepeatButtonDemoCtl>();
            containerRegistry.RegisterForNavigation<RichTextBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<SliderDemoCtl>();
            containerRegistry.RegisterForNavigation<TextBlockDemoCtl>();
            containerRegistry.RegisterForNavigation<ToggleButtonDemoCtl>();
            containerRegistry.RegisterForNavigation<ToolBarDemoCtl>();
            containerRegistry.RegisterForNavigation<TreeViewDemoCtl>();
            #endregion

            #region Controls
            containerRegistry.RegisterForNavigation<AnimationPathDemoCtl>();
            containerRegistry.RegisterForNavigation<BadgeDemoCtl>();
            containerRegistry.RegisterForNavigation<ButtonGroupDemoCtl>();
            containerRegistry.RegisterForNavigation<CalendarWithClockDemoCtl>();
            containerRegistry.RegisterForNavigation<CardDemoCtl>();
            containerRegistry.RegisterForNavigation<CarouselDemoCtl>();
            containerRegistry.RegisterForNavigation<ChatBubbleDemoCtl>();
            containerRegistry.RegisterForNavigation<CheckComboBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<CirclePanelDemoCtl>();
            containerRegistry.RegisterForNavigation<ClockDemoCtl>();
            containerRegistry.RegisterForNavigation<ColorPickerDemoCtl>();
            containerRegistry.RegisterForNavigation<ComboBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<CompareSliderDemoCtl>();
            containerRegistry.RegisterForNavigation<CoverFlowDemoCtl>();
            containerRegistry.RegisterForNavigation<CoverViewDemoCtl>();
            containerRegistry.RegisterForNavigation<DatePickerDemoCtl>();
            containerRegistry.RegisterForNavigation<DateTimePickerDemoCtl>();
            containerRegistry.RegisterForNavigation<DialogDemoCtl>();
            containerRegistry.RegisterForNavigation<DividerDemoCtl>();
            containerRegistry.RegisterForNavigation<DrawerDemoCtl>();
            containerRegistry.RegisterForNavigation<FlexPanelDemoCtl>();
            containerRegistry.RegisterForNavigation<FlipClockDemoCtl>();
            containerRegistry.RegisterForNavigation<FloatingBlockDemoCtl>();
            containerRegistry.RegisterForNavigation<GifImageDemoCtl>();
            containerRegistry.RegisterForNavigation<GotoTopDemoCtl>();
            containerRegistry.RegisterForNavigation<GravatarDemoCtl>();
            containerRegistry.RegisterForNavigation<GridDemoCtl>();
            containerRegistry.RegisterForNavigation<GrowlDemoCtl>();
            containerRegistry.RegisterForNavigation<HoneycombPanelDemoCtl>();
            containerRegistry.RegisterForNavigation<ImageBlockDemoCtl>();
            containerRegistry.RegisterForNavigation<ImageBrowserDemoCtl>();
            containerRegistry.RegisterForNavigation<ImageSelectorDemoCtl>();
            containerRegistry.RegisterForNavigation<LoadingDemoCtl>();
            containerRegistry.RegisterForNavigation<MagnifierDemoCtl>();
            containerRegistry.RegisterForNavigation<NotificationDemoCtl>();
            containerRegistry.RegisterForNavigation<NotifyIconDemoCtl>();
            containerRegistry.RegisterForNavigation<NumericUpDownDemoCtl>();
            containerRegistry.RegisterForNavigation<OutlineTextDemoCtl>();
            containerRegistry.RegisterForNavigation<PaginationDemoCtl>();
            containerRegistry.RegisterForNavigation<PasswordBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<PinBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<PoptipDemoCtl>();
            containerRegistry.RegisterForNavigation<PreviewSliderDemoCtl>();
            containerRegistry.RegisterForNavigation<ProgressBarDemoCtl>();
            containerRegistry.RegisterForNavigation<ProgressButtonDemoCtl>();
            containerRegistry.RegisterForNavigation<PropertyGridDemoCtl>();
            containerRegistry.RegisterForNavigation<RangeSliderDemoCtl>();
            containerRegistry.RegisterForNavigation<RateDemoCtl>();
            containerRegistry.RegisterForNavigation<RelativePanelDemoCtl>();
            containerRegistry.RegisterForNavigation<RunningBlockDemoCtl>();
            containerRegistry.RegisterForNavigation<ScreenshotDemoCtl>();
            containerRegistry.RegisterForNavigation<ScrollViewerDemoCtl>();
            containerRegistry.RegisterForNavigation<SearchBarDemoCtl>();
            containerRegistry.RegisterForNavigation<ShieldDemoCtl>();
            containerRegistry.RegisterForNavigation<SideMenuDemoCtl>();
            containerRegistry.RegisterForNavigation<SplitButtonDemoCtl>();
            containerRegistry.RegisterForNavigation<SpriteDemoCtl>();
            containerRegistry.RegisterForNavigation<StepBarDemoCtl>();
            containerRegistry.RegisterForNavigation<TabControlDemoCtl>();
            containerRegistry.RegisterForNavigation<TagDemoCtl>();
            containerRegistry.RegisterForNavigation<TextBoxDemoCtl>();
            containerRegistry.RegisterForNavigation<TimeBarDemoCtl>();
            containerRegistry.RegisterForNavigation<TimePickerDemoCtl>();
            containerRegistry.RegisterForNavigation<TransferDemoCtl>();
            containerRegistry.RegisterForNavigation<TransitioningContentControlDemoCtl>();
            containerRegistry.RegisterForNavigation<WaterfallPanelDemoCtl>();
            containerRegistry.RegisterForNavigation<WindowDemoCtl>();
            #endregion
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // Due to the duplication of the code, we can use one ViewModel for several Views
            ViewModelLocationProvider.Register<NativeComboBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();
            ViewModelLocationProvider.Register<ComboBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();
            ViewModelLocationProvider.Register<CheckComboBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();
            ViewModelLocationProvider.Register<GroupBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();

            ViewModelLocationProvider.Register<CardDemoCtl, CardDemoCtlViewModel>();
            ViewModelLocationProvider.Register<HoneycombPanelDemoCtl, CardDemoCtlViewModel>();

            ViewModelLocationProvider.Register<GrowlDemoCtl, GrowlDemoCtlViewModel>();
            ViewModelLocationProvider.Register<GrowlDemoWindow, GrowlDemoCtlViewModel>();

            ViewModelLocationProvider.Register<ListViewDemoCtl, ListViewModel>();
            ViewModelLocationProvider.Register<ListBoxDemoCtl, ListViewModel>();
            ViewModelLocationProvider.Register<TransferDemoCtl, ListViewModel>();

            ViewModelLocationProvider.Register<NativeWindowDemoCtl, NativeWindowDemoCtlViewModel>();
            ViewModelLocationProvider.Register<WindowDemoCtl, NativeWindowDemoCtlViewModel>();
        }
    }
}
