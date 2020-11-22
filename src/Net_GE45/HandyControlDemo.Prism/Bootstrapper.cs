using HandyControlDemo.ViewModels;
using HandyControlDemo.Views;
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
            // Main
            containerRegistry.RegisterForNavigation<ProjectsView>();
            containerRegistry.RegisterForNavigation<QQGroupView>();
            containerRegistry.RegisterForNavigation<WebsitesView>();
            containerRegistry.RegisterForNavigation<BlogsView>();
            containerRegistry.RegisterForNavigation<ContributorsView>();

            // Tools
            containerRegistry.RegisterForNavigation<EffectsDemoCtl>();
            containerRegistry.RegisterForNavigation<GeometryAnimationDemoCtl>();
            containerRegistry.RegisterForNavigation<HatchBrushGeneratorDemoCtl>();

            // Styles
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

            // Controls
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
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // Due to the duplication of the code, we can use one ViewModel for several Views
            ViewModelLocationProvider.Register<NativeComboBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();
            ViewModelLocationProvider.Register<ComboBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();
            ViewModelLocationProvider.Register<CheckComboBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();
            ViewModelLocationProvider.Register<GroupBoxDemoCtl, NativeComboBoxDemoCtlViewModel>();

        }
    }
}
