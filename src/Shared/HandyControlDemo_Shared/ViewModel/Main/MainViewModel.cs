using System;
using System.Collections.Generic;
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
        ///     demo信息
        /// </summary>
        private List<DemoInfoModel> _demoInfoList;

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
                DemoItemCurrent = null;
                foreach (var item in DemoInfoList)
                {
                    item.SelectedIndex = -1;
                }
            });

            DataList = dataService.GetDemoDataList();
            DemoInfoList = dataService.GetDemoInfo();
        }

        #region 属性

        /// <summary>
        ///     当前选中的demo项
        /// </summary>
        public DemoItemModel DemoItemCurrent { get; private set; }

        public DemoInfoModel DemoInfoCurrent { get; set; }

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

        /// <summary>
        ///     demo信息
        /// </summary>
        public List<DemoInfoModel> DemoInfoList
        {
            get => _demoInfoList;
#if netle40
            set => Set(nameof(DemoInfoList), ref _demoInfoList, value);
#else
            set => Set(ref _demoInfoList, value);
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

        public RelayCommand OpenPracticalDemoCmd => new Lazy<RelayCommand>(() =>
            new RelayCommand(OpenPracticalDemo)).Value;

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
            if (e.AddedItems[0] is DemoItemModel item)
            {
                if (Equals(DemoItemCurrent, item)) return;

                DemoItemCurrent = item;
                ContentTitle = item.Name;
                var obj = AssemblyHelper.ResolveByKey(item.TargetCtlName);
                var ctl = obj ?? AssemblyHelper.CreateInternalInstance($"UserControl.{item.TargetCtlName}");
                Messenger.Default.Send(ctl is IFull, MessageToken.FullSwitch);
                Messenger.Default.Send(ctl, MessageToken.LoadShowContent);
            }
        }

        private void OpenPracticalDemo()
        {
            Messenger.Default.Send<object>(null, MessageToken.ClearLeftSelected);
            Messenger.Default.Send(true, MessageToken.FullSwitch);
            Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{MessageToken.PracticalDemo}"), MessageToken.LoadShowContent);
        }

        #endregion
    }
}