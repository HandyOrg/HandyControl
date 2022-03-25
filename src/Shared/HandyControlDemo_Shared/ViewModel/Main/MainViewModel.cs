using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using HandyControl.Interactivity;
using HandyControlDemo.Data;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.Service;
using HandyControlDemo.Tools;
using HandyControlDemo.UserControl;

namespace HandyControlDemo.ViewModel;

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

    private readonly DataService _dataService;

    #endregion

    public MainViewModel(DataService dataService)
    {
        _dataService = dataService;

        UpdateMainContent();
        UpdateLeftContent();
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
#if NET40
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
#if NET40
        set => Set(nameof(ContentTitle), ref _contentTitle, value);
#else
        set => Set(ref _contentTitle, value);
#endif
    }

    /// <summary>
    ///     demo信息
    /// </summary>
    public ObservableCollection<DemoInfoModel> DemoInfoCollection { get; set; }

    #endregion

    #region 命令

    /// <summary>
    ///     切换例子命令
    /// </summary>
    public RelayCommand<SelectionChangedEventArgs> SwitchDemoCmd => new(SwitchDemo);

    public RelayCommand OpenPracticalDemoCmd => new(OpenPracticalDemo);

    public RelayCommand GlobalShortcutInfoCmd => new(() => Growl.Info("Global Shortcut Info"));

    public RelayCommand GlobalShortcutWarningCmd => new(() => Growl.Warning("Global Shortcut Warning"));

    public RelayCommand OpenDocCmd => new(() =>
    {
        ControlCommands.OpenLink.Execute(_dataService.GetDemoUrl(DemoInfoCurrent, DemoItemCurrent));
    });

    #endregion

    #region 方法

    private void UpdateMainContent()
    {
        Messenger.Default.Register<object>(this, MessageToken.LoadShowContent, obj =>
        {
            if (SubContent is IDisposable disposable)
            {
                disposable.Dispose();
            }
            SubContent = obj;
        }, true);
    }

    private void UpdateLeftContent()
    {
        //clear status
        Messenger.Default.Register<object>(this, MessageToken.ClearLeftSelected, obj =>
        {
            DemoItemCurrent = null;
            foreach (var item in DemoInfoCollection)
            {
                item.SelectedIndex = -1;
            }
        });

        Messenger.Default.Register<object>(this, MessageToken.LangUpdated, obj =>
        {
            if (DemoItemCurrent == null) return;
            ContentTitle = LangProvider.GetLang(DemoItemCurrent.Name);
        });

        //load items
        DemoInfoCollection = new ObservableCollection<DemoInfoModel>();
#if NET40
        Task.Factory.StartNew(() =>
#else
        Task.Run(() =>
#endif
        {
            DataList = _dataService.GetDemoDataList();

            foreach (var item in _dataService.GetDemoInfo())
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    DemoInfoCollection.Add(item);
                }), DispatcherPriority.ApplicationIdle);
            }
        });
    }

    /// <summary>
    ///     切换例子
    /// </summary>
    private void SwitchDemo(SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0) return;
        if (e.AddedItems[0] is DemoItemModel item)
        {
            if (Equals(DemoItemCurrent, item)) return;
            SwitchDemo(item);
        }
    }

    private void SwitchDemo(DemoItemModel item)
    {
        DemoItemCurrent = item;
        ContentTitle = LangProvider.GetLang(item.Name);
        var obj = AssemblyHelper.ResolveByKey(item.TargetCtlName);
        var ctl = obj ?? AssemblyHelper.CreateInternalInstance($"UserControl.{item.TargetCtlName}");
        Messenger.Default.Send(ctl is IFull, MessageToken.FullSwitch);
        Messenger.Default.Send(ctl, MessageToken.LoadShowContent);
    }

    private void OpenPracticalDemo()
    {
        Messenger.Default.Send<object>(null, MessageToken.ClearLeftSelected);
        Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{MessageToken.PracticalDemo}"), MessageToken.LoadShowContent);
        Messenger.Default.Send(true, MessageToken.FullSwitch);
    }

    #endregion
}
