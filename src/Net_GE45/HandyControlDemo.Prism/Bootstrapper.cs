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
            containerRegistry.RegisterForNavigation<ProjectsView>();
            containerRegistry.RegisterForNavigation<QQGroupView>();
            containerRegistry.RegisterForNavigation<WebsitesView>();
            containerRegistry.RegisterForNavigation<BlogsView>();
            containerRegistry.RegisterForNavigation<ContributorsView>();
            containerRegistry.RegisterForNavigation<BorderDemoCtl>();
            containerRegistry.RegisterForNavigation<BrushDemoCtl>();
            containerRegistry.RegisterForNavigation<ButtonDemoCtl>();
        }
    }
}
