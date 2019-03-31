using System;
using System.Reflection.Emit;
using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using HandyControlDemo.Data;
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
            SimpleIoc.Default.Register(() => new GrowlDemoViewModel(), "GrowlDemo");
            SimpleIoc.Default.Register(() => new GrowlDemoViewModel(MessageToken.GrowlDemoPanel), "GrowlDemoWithToken");
            SimpleIoc.Default.Register<ImageBrowserDemoViewModel>();
            SimpleIoc.Default.Register<ComboBoxDemoViewModel>();
            SimpleIoc.Default.Register<WindowDemoViewModel>();
            SimpleIoc.Default.Register<ContributorsViewModel>();
            SimpleIoc.Default.Register<StepBarDemoViewModel>();
            SimpleIoc.Default.Register<PaginationDemoViewModel>();
            SimpleIoc.Default.Register<ChatBoxViewModel>();
            SimpleIoc.Default.Register<CoverViewModel>();
            SimpleIoc.Default.Register<DialogDemoViewModel>();
            SimpleIoc.Default.Register<SearchBarDemoViewModel>();
            SimpleIoc.Default.Register<NotifyIconDemoViewModel>();
        }

        public static ViewModelLocator Instance => new Lazy<ViewModelLocator>(() =>
            Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

        #region Vm

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public GrowlDemoViewModel GrowlDemo => ServiceLocator.Current.GetInstance<GrowlDemoViewModel>("GrowlDemo");

        public GrowlDemoViewModel GrowlDemoWithToken => ServiceLocator.Current.GetInstance<GrowlDemoViewModel>("GrowlDemoWithToken");

        public ImageBrowserDemoViewModel ImageBrowserDemo => ServiceLocator.Current.GetInstance<ImageBrowserDemoViewModel>();

        public ComboBoxDemoViewModel ComboBoxDemo => ServiceLocator.Current.GetInstance<ComboBoxDemoViewModel>();

        public WindowDemoViewModel WindowDemo => ServiceLocator.Current.GetInstance<WindowDemoViewModel>();

        public ContributorsViewModel ContributorsView => ServiceLocator.Current.GetInstance<ContributorsViewModel>();

        public StepBarDemoViewModel StepBarDemo => ServiceLocator.Current.GetInstance<StepBarDemoViewModel>();

        public PaginationDemoViewModel PaginationDemo => ServiceLocator.Current.GetInstance<PaginationDemoViewModel>();

        public ChatBoxViewModel ChatBox => new ChatBoxViewModel();

        public CoverViewModel CoverView => ServiceLocator.Current.GetInstance<CoverViewModel>();

        public DialogDemoViewModel DialogDemo => ServiceLocator.Current.GetInstance<DialogDemoViewModel>();

        public SearchBarDemoViewModel SearchBarDemo => ServiceLocator.Current.GetInstance<SearchBarDemoViewModel>();

        public NotifyIconDemoViewModel NotifyIconDemo => ServiceLocator.Current.GetInstance<NotifyIconDemoViewModel>();

        #endregion
    }
}