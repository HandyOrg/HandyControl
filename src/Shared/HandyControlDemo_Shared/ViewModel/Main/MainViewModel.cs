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
    private object _contentTitle;
    private object _subContent;
    private bool _isCodeOpened;

    private readonly DataService _dataService;

    public MainViewModel(DataService dataService)
    {
        _dataService = dataService;

        UpdateMainContent();
        UpdateLeftContent();
    }

    public DemoItemModel DemoItemCurrent { get; private set; }

    public DemoInfoModel DemoInfoCurrent { get; set; }

    public object SubContent
    {
        get => _subContent;
#if NET40
        set => Set(nameof(SubContent), ref _subContent, value);
#else
        set => Set(ref _subContent, value);
#endif
    }

    public object ContentTitle
    {
        get => _contentTitle;
#if NET40
        set => Set(nameof(ContentTitle), ref _contentTitle, value);
#else
        set => Set(ref _contentTitle, value);
#endif
    }

    public bool IsCodeOpened
    {
        get => _isCodeOpened;
#if NET40
        set => Set(nameof(IsCodeOpened), ref _isCodeOpened, value);
#else
        set => Set(ref _isCodeOpened, value);
#endif
    }

    public ObservableCollection<DemoInfoModel> DemoInfoCollection { get; set; }

    public RelayCommand<SelectionChangedEventArgs> SwitchDemoCmd => new(SwitchDemo);

    public RelayCommand OpenPracticalDemoCmd => new(OpenPracticalDemo);

    public RelayCommand GlobalShortcutInfoCmd => new(() => Growl.Info("Global Shortcut Info"));

    public RelayCommand GlobalShortcutWarningCmd => new(() => Growl.Warning("Global Shortcut Warning"));

    public RelayCommand OpenDocCmd => new(() =>
    {
        if (DemoItemCurrent is null)
        {
            return;
        }

        ControlCommands.OpenLink.Execute(_dataService.GetDemoUrl(DemoInfoCurrent, DemoItemCurrent));
    });

    public RelayCommand OpenCodeCmd => new(() =>
    {
        if (DemoItemCurrent is null)
        {
            return;
        }

        IsCodeOpened = !IsCodeOpened;
    });

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
}
