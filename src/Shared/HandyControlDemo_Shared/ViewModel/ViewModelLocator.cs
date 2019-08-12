﻿using System;
using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using HandyControlDemo.Data;
using HandyControlDemo.Service;
using HandyControlDemo.ViewModel.Basic;
using HandyControlDemo.ViewModel.Controls;

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
            SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(ServiceLocator.Current.GetInstance<DataService>().GetContributorDataList), "Contributors");
            SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(ServiceLocator.Current.GetInstance<DataService>().GetBlogDataList), "Blogs");
            SimpleIoc.Default.Register(() => new ItemsDisplayViewModel(ServiceLocator.Current.GetInstance<DataService>().GetProjectDataList), "Projects");
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

        public ItemsDisplayViewModel ContributorsView => ServiceLocator.Current.GetInstance<ItemsDisplayViewModel>("Contributors");

        public ItemsDisplayViewModel BlogsView => ServiceLocator.Current.GetInstance<ItemsDisplayViewModel>("Blogs");

        public ItemsDisplayViewModel ProjectsView => ServiceLocator.Current.GetInstance<ItemsDisplayViewModel>("Projects");

        public StepBarDemoViewModel StepBarDemo => ServiceLocator.Current.GetInstance<StepBarDemoViewModel>();

        public PaginationDemoViewModel PaginationDemo => ServiceLocator.Current.GetInstance<PaginationDemoViewModel>();

        public ChatBoxViewModel ChatBox => new ChatBoxViewModel();

        public CoverViewModel CoverView => ServiceLocator.Current.GetInstance<CoverViewModel>();

        public DialogDemoViewModel DialogDemo => ServiceLocator.Current.GetInstance<DialogDemoViewModel>();

        public SearchBarDemoViewModel SearchBarDemo => ServiceLocator.Current.GetInstance<SearchBarDemoViewModel>();

        public NotifyIconDemoViewModel NotifyIconDemo => ServiceLocator.Current.GetInstance<NotifyIconDemoViewModel>();

        public InteractiveDialogViewModel InteractiveDialog => ServiceLocator.Current.GetInstance<InteractiveDialogViewModel>();

        public BadgeDemoViewModel BadgeDemo => ServiceLocator.Current.GetInstance<BadgeDemoViewModel>();

        public SideMenuDemoViewModel SideMenuDemo => ServiceLocator.Current.GetInstance<SideMenuDemoViewModel>();

        public TabControlDemoViewModel TabControlDemo => ServiceLocator.Current.GetInstance<TabControlDemoViewModel>();

        public NoUserViewModel NoUser => ServiceLocator.Current.GetInstance<NoUserViewModel>();

        public CardDemoViewModel CardDemo => new CardDemoViewModel(ServiceLocator.Current.GetInstance<DataService>());

        #endregion
    }
}