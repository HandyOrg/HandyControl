using System;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HandyControlDemo.Data;
using HandyControlDemo.UserControl;

namespace HandyControlDemo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region 字段

        /// <summary>
        ///     内容标题
        /// </summary>
        private object _contentTitle;

        /// <summary>
        ///     左侧主内容
        /// </summary>
        private object _leftMainContent;

        /// <summary>
        ///     主内容
        /// </summary>
        private object _mainContent;

        /// <summary>
        ///     子内容
        /// </summary>
        private object _subContent;

        /// <summary>
        ///     非用户区域内容
        /// </summary>
        private object _noUserContent;

        #endregion

        public MainViewModel()
        {
            LeftMainContent = ControlLocator.Instance.LeftMainContent;
            MainContent = ControlLocator.Instance.MainContent;
            NoUserContent = ControlLocator.Instance.NoUserContent;

            Messenger.Default.Register<object>(this, MessageToken.LoadGrowlDemoCtl, obj => SubContent = ControlLocator.Instance.GrowlDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadLoadingDemoCtl, obj => SubContent = ControlLocator.Instance.LoadingDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadImageBrowserDemoCtl, obj => SubContent = ControlLocator.Instance.ImageBrowserDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadColorPickerDemoCtl, obj => SubContent = ControlLocator.Instance.ColorPickerDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadCarouselDemoCtl, obj => SubContent = ControlLocator.Instance.CarouselDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadCompareSliderDemoCtl, obj => SubContent = ControlLocator.Instance.CompareSliderDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadTimeBarDemoCtl, obj => SubContent = ControlLocator.Instance.TimeBarDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadPaginationDemoCtl, obj => SubContent = ControlLocator.Instance.PaginationDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadExpanderDemoCtl, obj => SubContent = ControlLocator.Instance.ExpanderDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadProgressBarDemoCtl, obj => SubContent = ControlLocator.Instance.ProgressBarDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadAnimationPathDemoCtl, obj => SubContent = ControlLocator.Instance.AnimationPathDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadButtonDemoCtl, obj => SubContent = ControlLocator.Instance.ButtonDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadToggleButtonDemoCtl, obj => SubContent = ControlLocator.Instance.ToggleButtonDemoCtl);
            Messenger.Default.Register<object>(this, MessageToken.LoadTabControlDemoCtl, obj => SubContent = ControlLocator.Instance.TabControlDemoCtl);
        }

        #region 属性

        /// <summary>
        ///     非用户区域内容
        /// </summary>
        public object NoUserContent
        {
            get => _noUserContent;
            set => Set(ref _noUserContent, value);
        }

        /// <summary>
        ///     左侧主内容
        /// </summary>
        public object LeftMainContent
        {
            get => _leftMainContent;
            set => Set(ref _leftMainContent, value);
        }

        /// <summary>
        ///     主内容
        /// </summary>
        public object MainContent
        {
            get => _mainContent;
            set => Set(ref _mainContent, value);
        }

        /// <summary>
        ///     子内容
        /// </summary>
        public object SubContent
        {
            get => _subContent;
            set => Set(ref _subContent, value);
        }

        /// <summary>
        ///     内容标题
        /// </summary>
        public object ContentTitle
        {
            get => _contentTitle;
            set => Set(ref _contentTitle, value);
        }

        #endregion

        #region 命令

        /// <summary>
        ///     切换例子命令
        /// </summary>
        public RelayCommand<RoutedPropertyChangedEventArgs<object>> SwitchDemoCmd =>
            new Lazy<RelayCommand<RoutedPropertyChangedEventArgs<object>>>(() =>
                new RelayCommand<RoutedPropertyChangedEventArgs<object>>(SwitchDemo)).Value;

        #endregion

        #region 方法

        /// <summary>
        ///     切换例子
        /// </summary>
        private void SwitchDemo(RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item)
            {
                if (item.Tag is string tag)
                {
                    ContentTitle = item.Header;
                    Messenger.Default.Send<object>(null, tag);
                }
                else
                {
                    ContentTitle = null;
                    SubContent = null;
                }
            }
        }

        #endregion
    }
}