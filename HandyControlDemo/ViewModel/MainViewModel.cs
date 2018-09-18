using System;
using System.Windows;
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

        #endregion

        public MainViewModel()
        {
            Messenger.Default.Register<object>(this, MessageToken.LoadShowContent, obj => SubContent = obj);
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