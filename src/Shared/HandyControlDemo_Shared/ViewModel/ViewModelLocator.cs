using System;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<DataService>();
            var dataService = SimpleIoc.Default.GetInstance<DataService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register(() => new GrowlDemoViewModel(), "GrowlDemo");
            SimpleIoc.Default.Register(() => new GrowlDemoViewModel(MessageToken.GrowlDemoPanel), "GrowlDemoWithToken");
            SimpleIoc.Default.Register<ImageBrowserDemoViewModel>();
            SimpleIoc.Default.Register<ComboBoxDemoViewModel>();
            SimpleIoc.Default.Register<WindowDemoViewModel>();
            SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(dataService.GetBlogDataList), "Blogs");
            SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(dataService.GetProjectDataList), "Projects");
            SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(dataService.GetWebsiteDataList), "Websites");
            SimpleIoc.Default.Register<StepBarDemoViewModel>();
            SimpleIoc.Default.Register<PaginationDemoViewModel>();
            SimpleIoc.Default.Register<ChatBoxViewModel>();
            SimpleIoc.Default.Register<CoverViewModel>();
            SimpleIoc.Default.Register<DialogDemoViewModel>();
            SimpleIoc.Default.Register<SearchBarDemoViewModel>();
            SimpleIoc.Default.Register<NotifyIconDemoViewModel>();
            SimpleIoc.Default.Register<InteractiveDialogViewModel>();
            SimpleIoc.Default.Register<BadgeDemoViewModel>();
            SimpleIoc.Default.Register<SideMenuDemoViewModel>();
            SimpleIoc.Default.Register<TabControlDemoViewModel>();
            SimpleIoc.Default.Register<NoUserViewModel>();
            SimpleIoc.Default.Register<CardDemoViewModel>();
            SimpleIoc.Default.Register<SpriteDemoViewModel>();
            SimpleIoc.Default.Register<NotificationDemoViewModel>();
            SimpleIoc.Default.Register<SplitButtonDemoViewModel>();
        }

        public static ViewModelLocator Instance => new Lazy<ViewModelLocator>(() =>
            Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

        #region Vm

        public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();

        public GrowlDemoViewModel GrowlDemo => SimpleIoc.Default.GetInstance<GrowlDemoViewModel>("GrowlDemo");

        public GrowlDemoViewModel GrowlDemoWithToken => SimpleIoc.Default.GetInstance<GrowlDemoViewModel>("GrowlDemoWithToken");

        public ImageBrowserDemoViewModel ImageBrowserDemo => SimpleIoc.Default.GetInstance<ImageBrowserDemoViewModel>();

        public ComboBoxDemoViewModel ComboBoxDemo => SimpleIoc.Default.GetInstance<ComboBoxDemoViewModel>();

        public WindowDemoViewModel WindowDemo => SimpleIoc.Default.GetInstance<WindowDemoViewModel>();

        public ItemsDisplayViewModel ContributorsView => new ItemsDisplayViewModel(SimpleIoc.Default.GetInstance<DataService>().GetContributorDataList);

        public ItemsDisplayViewModel BlogsView => SimpleIoc.Default.GetInstance<ItemsDisplayViewModel>("Blogs");

        public ItemsDisplayViewModel ProjectsView => SimpleIoc.Default.GetInstance<ItemsDisplayViewModel>("Projects");

        public ItemsDisplayViewModel WebsitesView => SimpleIoc.Default.GetInstance<ItemsDisplayViewModel>("Websites");

        public StepBarDemoViewModel StepBarDemo => SimpleIoc.Default.GetInstance<StepBarDemoViewModel>();

        public PaginationDemoViewModel PaginationDemo => SimpleIoc.Default.GetInstance<PaginationDemoViewModel>();

        public ChatBoxViewModel ChatBox => new ChatBoxViewModel();

        public CoverViewModel CoverView => SimpleIoc.Default.GetInstance<CoverViewModel>();

        public DialogDemoViewModel DialogDemo => SimpleIoc.Default.GetInstance<DialogDemoViewModel>();

        public SearchBarDemoViewModel SearchBarDemo => SimpleIoc.Default.GetInstance<SearchBarDemoViewModel>();

        public NotifyIconDemoViewModel NotifyIconDemo => SimpleIoc.Default.GetInstance<NotifyIconDemoViewModel>();

        public InteractiveDialogViewModel InteractiveDialog => SimpleIoc.Default.GetInstance<InteractiveDialogViewModel>();

        public BadgeDemoViewModel BadgeDemo => SimpleIoc.Default.GetInstance<BadgeDemoViewModel>();

        public SideMenuDemoViewModel SideMenuDemo => SimpleIoc.Default.GetInstance<SideMenuDemoViewModel>();

        public TabControlDemoViewModel TabControlDemo => new TabControlDemoViewModel(SimpleIoc.Default.GetInstance<DataService>());

        public NoUserViewModel NoUser => SimpleIoc.Default.GetInstance<NoUserViewModel>();

        public CardDemoViewModel CardDemo => new CardDemoViewModel(SimpleIoc.Default.GetInstance<DataService>());

        public SpriteDemoViewModel SpriteDemo => SimpleIoc.Default.GetInstance<SpriteDemoViewModel>();

        public NotificationDemoViewModel NotificationDemo => SimpleIoc.Default.GetInstance<NotificationDemoViewModel>();

        public SplitButtonDemoViewModel SplitButtonDemo => SimpleIoc.Default.GetInstance<SplitButtonDemoViewModel>();

        #endregion
    }
}