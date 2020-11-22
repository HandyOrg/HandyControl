using HandyControlDemo.Views;
using Prism.DryIoc;
using Prism.Ioc;
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

        }
    }
}
