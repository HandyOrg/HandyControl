using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using HandyControlDemo.Service;
using Microsoft.Practices.ServiceLocation;

namespace HandyControlDemo.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<DataService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<GrowlDemoViewModel>();
            SimpleIoc.Default.Register<ImageBrowserDemoViewModel>();
            SimpleIoc.Default.Register<ComboBoxDemoViewModel>();
            SimpleIoc.Default.Register<WindowDemoViewModel>();
            SimpleIoc.Default.Register<ContributorsViewModel>();
            SimpleIoc.Default.Register<StepBarDemoViewModel>();
            SimpleIoc.Default.Register<PaginationDemoViewModel>();
        }

        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance =>
            _instance ?? (_instance = Application.Current.TryFindResource("Locator") as ViewModelLocator);

        #region Vm

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public GrowlDemoViewModel GrowlDemo => ServiceLocator.Current.GetInstance<GrowlDemoViewModel>();

        public ImageBrowserDemoViewModel ImageBrowserDemo => ServiceLocator.Current.GetInstance<ImageBrowserDemoViewModel>();

        public ComboBoxDemoViewModel ComboBoxDemo => ServiceLocator.Current.GetInstance<ComboBoxDemoViewModel>();

        public WindowDemoViewModel WindowDemo => ServiceLocator.Current.GetInstance<WindowDemoViewModel>();

        public ContributorsViewModel ContributorsView => ServiceLocator.Current.GetInstance<ContributorsViewModel>();

        public StepBarDemoViewModel StepBarDemo => ServiceLocator.Current.GetInstance<StepBarDemoViewModel>();

        public PaginationDemoViewModel PaginationDemo => ServiceLocator.Current.GetInstance<PaginationDemoViewModel>();

        #endregion
    }
}