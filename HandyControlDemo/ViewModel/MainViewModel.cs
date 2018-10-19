using System;
using System.Collections.Generic;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HandyControlDemo.Data;
using HandyControlDemo.Tools;

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
        ///     子内容
        /// </summary>
        private object _subContent;

        /// <summary>
        ///     数据列表
        /// </summary>
        private List<DemoDataModel> _dataList;

        #endregion

        public MainViewModel()
        {
            Messenger.Default.Register<object>(this, MessageToken.LoadShowContent, obj => SubContent = obj);

            var list = new List<DemoDataModel>();
            for (var i = 1; i <= 6; i++)
            {
                var dataList = new List<DemoDataModel>();
                for (int j = 0; j < 3; j++)
                {
                    dataList.Add(new DemoDataModel
                    {
                        Index = j,
                        IsSelected = j % 2 == 0,
                        Name = $"SubName{j}",
                        Type = (DemoType)j
                    });
                }
                var model = new DemoDataModel
                {
                    Index = i,
                    IsSelected = i % 2 == 0,
                    Name = $"Name{i}",
                    Type = (DemoType)i,
                    DataList = dataList
                };
                list.Add(model);
            }

            DataList = list;
        }

        #region 属性

        /// <summary>
        ///     数据列表
        /// </summary>
        public List<DemoDataModel> DataList
        {
            get => _dataList;
            set => Set(ref _dataList, value);
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
                    if (item.Content.Equals(ContentTitle)) return;
                    ContentTitle = item.Content;
                    Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{tag}"), MessageToken.LoadShowContent);
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