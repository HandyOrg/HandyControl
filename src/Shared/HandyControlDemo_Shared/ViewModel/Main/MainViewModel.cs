using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HandyControlDemo.Data;
using HandyControlDemo.Service;
using HandyControlDemo.Tools;

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
        public RelayCommand<SelectionChangedEventArgs> SwitchDemoCmd =>
            new Lazy<RelayCommand<SelectionChangedEventArgs>>(() =>
                new RelayCommand<SelectionChangedEventArgs>(SwitchDemo)).Value;

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
                    Messenger.Default.Send(false, MessageToken.FullSwitch);
                    Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{tag}"), MessageToken.LoadShowContent);
                }
                else
                {
                    _listBoxItemCurrent = null;
                    ContentTitle = null;
                    SubContent = null;
                }
            }
        }

        #endregion
    }
}