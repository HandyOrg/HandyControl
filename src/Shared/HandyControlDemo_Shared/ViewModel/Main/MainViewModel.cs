using System;
using System.Windows.Controls;
#if netle40
using GalaSoft.MvvmLight.Command;
#else
using GalaSoft.MvvmLight.CommandWpf;
#endif
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HandyControlDemo.Data;
using HandyControlDemo.Service;
using HandyControlDemo.Tools;
using HandyControlDemo.UserControl;

namespace HandyControlDemo.ViewModel
{
    public class MainViewModel : DemoViewModelBase<DemoDataModel>
    {
        #region 字段

        /// <summary>
        ///     内容标题
        /// </summary>
        private object _contentTitle;

        /// <summary>
        ///     子内容
        /// </summary>
        private object _subContent;

        /// <summary>
        ///     当前选中的列表项
        /// </summary>
        private ListBoxItem _listBoxItemCurrent;

        #endregion

        public MainViewModel(DataService dataService)
        {
            Messenger.Default.Register<object>(this, MessageToken.LoadShowContent, obj =>
            {
                if (SubContent is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                SubContent = obj;
            });
            Messenger.Default.Register<object>(this, MessageToken.ClearLeftSelected, obj =>
            {
                if (_listBoxItemCurrent == null) return;
                _listBoxItemCurrent.IsSelected = false;
                _listBoxItemCurrent = null;
            });
            DataList = dataService.GetDemoDataList();
        }

        #region 属性

        /// <summary>
        ///     子内容
        /// </summary>
        public object SubContent
        {
            get => _subContent;
#if netle40
            set => Set(nameof(SubContent), ref _subContent, value);
#else
            set => Set(ref _subContent, value);
#endif
        }

        /// <summary>
        ///     内容标题
        /// </summary>
        public object ContentTitle
        {
            get => _contentTitle;
#if netle40
            set => Set(nameof(ContentTitle), ref _contentTitle, value);
#else
            set => Set(ref _contentTitle, value);
#endif
        }

        #endregion

        #region 命令

        /// <summary>
        ///     切换例子命令
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> SwitchDemoCmd =>
            new Lazy<RelayCommand<SelectionChangedEventArgs>>(() =>
                new RelayCommand<SelectionChangedEventArgs>(SwitchDemo)).Value;

        /// <summary>
        ///     打开概览命令
        /// </summary>
        public RelayCommand OpenOverviewCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(OpenOverview)).Value;

        public RelayCommand GlobalShortcutInfoCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Info("Global Shortcut Info"))).Value;

        public RelayCommand GlobalShortcutWarningCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(() => Growl.Warning("Global Shortcut Warning"))).Value;

        #endregion

        #region 方法

        /// <summary>
        ///     切换例子
        /// </summary>
        private void SwitchDemo(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is ListBoxItem item)
            {
                if (item.Tag is string tag)
                {
                    if (Equals(_listBoxItemCurrent, item)) return;
                    _listBoxItemCurrent = item;
                    ContentTitle = item.Content;
                    var obj = AssemblyHelper.ResolveByKey(tag);
                    var ctl = obj ?? AssemblyHelper.CreateInternalInstance($"UserControl.{tag}");
                    Messenger.Default.Send(ctl is IFull, MessageToken.FullSwitch);
                    Messenger.Default.Send(ctl, MessageToken.LoadShowContent);
                }
                else
                {
                    _listBoxItemCurrent = null;
                    ContentTitle = null;
                    SubContent = null;
                }
            }
        }

        /// <summary>
        ///     打开概览
        /// </summary>
        private void OpenOverview()
        {
            Messenger.Default.Send<object>(null, MessageToken.ClearLeftSelected);
            Messenger.Default.Send(true, MessageToken.FullSwitch);
            Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{MessageToken.OverView}"), MessageToken.LoadShowContent);
        }

        #endregion
    }
}