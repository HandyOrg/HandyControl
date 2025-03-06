using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using HandyControlDemo.Data;
using HandyControlDemo.Properties.Langs;
using HandyControlDemo.Service;
using HandyControlDemo.Tools;

namespace HandyControlDemo.ViewModel;

public class MainViewModel : DemoViewModelBase<DemoDataModel>
{
    private readonly DataService _dataService;

    private object? _contentTitle;
    private object? _subContent;

    public MainViewModel(DataService dataService)
    {
        _dataService = dataService;

        UpdateMainContent();
        UpdateLeftContent();
    }

    public DemoItemModel? DemoItemCurrent { get; private set; }

    public DemoInfoModel? DemoInfoCurrent { get; set; }

    public object? SubContent
    {
        get => _subContent;
        set => SetProperty(ref _subContent, value);
    }

    public object? ContentTitle
    {
        get => _contentTitle;
        set => SetProperty(ref _contentTitle, value);
    }

    public ObservableCollection<DemoInfoModel> DemoInfoCollection { get; set; } = [];

    private void UpdateMainContent()
    {
        WeakReferenceMessenger.Default.Register<DemoItemModel, string>(
            recipient: this,
            token: MessageToken.SwitchDemo,
            handler: (_, message) => SwitchDemo(message)
        );
    }

    private void UpdateLeftContent()
    {
        //clear status
        WeakReferenceMessenger.Default.Register<object, string>(this, MessageToken.ClearLeftSelected, (_, _) =>
        {
            DemoItemCurrent = null;
            foreach (var item in DemoInfoCollection)
            {
                item.SelectedIndex = -1;
            }
        });

        WeakReferenceMessenger.Default.Register<object, string>(this, MessageToken.LangUpdated, (_, _) =>
        {
            if (DemoItemCurrent == null)
            {
                return;
            }

            ContentTitle = Lang.ResourceManager.GetString(DemoItemCurrent.Name, Lang.Culture);
        });

        //load items
        DemoInfoCollection = [];
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            DataList = _dataService.GetDemoDataList();

            foreach (var item in _dataService.GetDemoInfo())
            {
                DemoInfoCollection.Add(item);
            }

            if (DemoInfoCollection.Any() && DemoInfoCollection.First().DemoItemList.Any())
            {
                SwitchDemo(DemoInfoCollection.First().DemoItemList.First());
            }
        });
    }

    private void SwitchDemo(DemoItemModel item)
    {
        if (SubContent is IDisposable disposable)
        {
            disposable.Dispose();
        }

        DemoItemCurrent = item;
        ContentTitle = Lang.ResourceManager.GetString(item.Name, Lang.Culture);
        object? demoControl = AssemblyHelper.ResolveByKey(item.TargetCtlName) ??
                              AssemblyHelper.CreateInternalInstance($"UserControl.{item.TargetCtlName}");
        SubContent = demoControl;
    }
}
