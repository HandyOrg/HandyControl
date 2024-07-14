using System;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using HandyControlDemo.Data;
using HandyControlDemo.Service;

namespace HandyControlDemo.ViewModel;

public class ViewModelLocator
{
    private static readonly Lazy<ViewModelLocator> InstanceInternal = new(() =>
        Application.Current.TryFindResource("Locator") as ViewModelLocator, isThreadSafe: true);

    public static ViewModelLocator Instance => InstanceInternal.Value;

    public ViewModelLocator()
    {
        SimpleIoc.Default.Register<DataService>();
        var dataService = SimpleIoc.Default.GetInstance<DataService>();

        SimpleIoc.Default.Register<MainViewModel>();
        SimpleIoc.Default.Register(() => new GrowlDemoViewModel(), "GrowlDemo");
        SimpleIoc.Default.Register(() => new GrowlDemoViewModel(MessageToken.GrowlDemoPanel), "GrowlDemoWithToken");
        SimpleIoc.Default.Register<ImageBrowserDemoViewModel>();
        SimpleIoc.Default.Register<WindowDemoViewModel>();
        SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(dataService.GetBlogDataList), "Blogs");
        SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(dataService.GetProjectDataList), "Projects");
        SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(dataService.GetWebsiteDataList), "Websites");
        SimpleIoc.Default.Register<StepBarDemoViewModel>();
        SimpleIoc.Default.Register<PaginationDemoViewModel>();
        SimpleIoc.Default.Register<ChatBoxViewModel>();
        SimpleIoc.Default.Register<CoverViewModel>();
        SimpleIoc.Default.Register<DialogDemoViewModel>();
        SimpleIoc.Default.Register<NotifyIconDemoViewModel>();
        SimpleIoc.Default.Register<InteractiveDialogViewModel>();
        SimpleIoc.Default.Register<BadgeDemoViewModel>();
        SimpleIoc.Default.Register<SideMenuDemoViewModel>();
        SimpleIoc.Default.Register<TabControlDemoViewModel>();
        SimpleIoc.Default.Register<NonClientAreaViewModel>();
        SimpleIoc.Default.Register<CardDemoViewModel>();
        SimpleIoc.Default.Register<SpriteDemoViewModel>();
        SimpleIoc.Default.Register<NotificationDemoViewModel>();
        SimpleIoc.Default.Register<SplitButtonDemoViewModel>();
        SimpleIoc.Default.Register<TagDemoViewModel>();
        SimpleIoc.Default.Register<AutoCompleteTextBoxDemoViewModel>();
        SimpleIoc.Default.Register<InputElementDemoViewModel>();
    }

    #region Vm

    public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();

    public GrowlDemoViewModel GrowlDemo => SimpleIoc.Default.GetInstance<GrowlDemoViewModel>("GrowlDemo");

    public GrowlDemoViewModel GrowlDemoWithToken => SimpleIoc.Default.GetInstance<GrowlDemoViewModel>("GrowlDemoWithToken");

    public ImageBrowserDemoViewModel ImageBrowserDemo => SimpleIoc.Default.GetInstance<ImageBrowserDemoViewModel>();

    public WindowDemoViewModel WindowDemo => SimpleIoc.Default.GetInstance<WindowDemoViewModel>();

    public ItemsDisplayViewModel ContributorsView => new(SimpleIoc.Default.GetInstance<DataService>().GetContributorDataList);

    public ItemsDisplayViewModel BlogsView => SimpleIoc.Default.GetInstance<ItemsDisplayViewModel>("Blogs");

    public ItemsDisplayViewModel ProjectsView => SimpleIoc.Default.GetInstance<ItemsDisplayViewModel>("Projects");

    public ItemsDisplayViewModel WebsitesView => SimpleIoc.Default.GetInstance<ItemsDisplayViewModel>("Websites");

    public StepBarDemoViewModel StepBarDemo => SimpleIoc.Default.GetInstance<StepBarDemoViewModel>();

    public PaginationDemoViewModel PaginationDemo => SimpleIoc.Default.GetInstance<PaginationDemoViewModel>();

    public ChatBoxViewModel ChatBox => new();

    public CoverViewModel CoverView => SimpleIoc.Default.GetInstance<CoverViewModel>();

    public DialogDemoViewModel DialogDemo => SimpleIoc.Default.GetInstance<DialogDemoViewModel>();

    public NotifyIconDemoViewModel NotifyIconDemo => SimpleIoc.Default.GetInstance<NotifyIconDemoViewModel>();

    public InteractiveDialogViewModel InteractiveDialog => SimpleIoc.Default.GetInstance<InteractiveDialogViewModel>();

    public BadgeDemoViewModel BadgeDemo => SimpleIoc.Default.GetInstance<BadgeDemoViewModel>();

    public SideMenuDemoViewModel SideMenuDemo => SimpleIoc.Default.GetInstance<SideMenuDemoViewModel>();

    public TabControlDemoViewModel TabControlDemo => new(SimpleIoc.Default.GetInstance<DataService>());

    public NonClientAreaViewModel NoUser => SimpleIoc.Default.GetInstance<NonClientAreaViewModel>();

    public CardDemoViewModel CardDemo => new(SimpleIoc.Default.GetInstance<DataService>());

    public SpriteDemoViewModel SpriteDemo => SimpleIoc.Default.GetInstance<SpriteDemoViewModel>();

    public NotificationDemoViewModel NotificationDemo => SimpleIoc.Default.GetInstance<NotificationDemoViewModel>();

    public SplitButtonDemoViewModel SplitButtonDemo => SimpleIoc.Default.GetInstance<SplitButtonDemoViewModel>();

    public TagDemoViewModel TagDemo => new(SimpleIoc.Default.GetInstance<DataService>());

    public AutoCompleteTextBoxDemoViewModel AutoCompleteTextBoxDemo => new(SimpleIoc.Default.GetInstance<DataService>());

    public InputElementDemoViewModel InputElementDemo => new();

    #endregion
}
