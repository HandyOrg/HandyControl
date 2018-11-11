using System;
using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using HandyControlDemo.Service;

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
        }

        public static ViewModelLocator Instance => new Lazy<ViewModelLocator>(() =>
            Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

        #region Vm

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public GrowlDemoViewModel GrowlDemo => ServiceLocator.Current.GetInstance<GrowlDemoViewModel>();

        public ImageBrowserDemoViewModel ImageBrowserDemo => ServiceLocator.Current.GetInstance<ImageBrowserDemoViewModel>();

        public ComboBoxDemoViewModel ComboBoxDemo => ServiceLocator.Current.GetInstance<ComboBoxDemoViewModel>();

        public WindowDemoViewModel WindowDemo => ServiceLocator.Current.GetInstance<WindowDemoViewModel>();

        public ContributorsViewModel ContributorsView => ServiceLocator.Current.GetInstance<ContributorsViewModel>();

        public StepBarDemoViewModel StepBarDemo => ServiceLocator.Current.GetInstance<StepBarDemoViewModel>();

        #endregion
    }
}