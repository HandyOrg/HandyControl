using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using HandyControl.Data;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class PaginationDemo : Avalonia.Controls.UserControl
{
    private readonly List<DemoDataModel> _totalDataList;

    public static readonly DirectProperty<PaginationDemo, ObservableCollection<DemoDataModel>> DataListProperty =
        AvaloniaProperty.RegisterDirect<PaginationDemo, ObservableCollection<DemoDataModel>>(nameof(DataList), o => o.DataList);

    public PaginationDemo()
    {
        InitializeComponent();

        DataContext = this;

        _totalDataList = BuildDataList(100);
        DataList = new ObservableCollection<DemoDataModel>();
        LoadPage(1, 10);
    }

    public ObservableCollection<DemoDataModel> DataList { get; }

    private void Pagination_OnPageUpdated(object? sender, FunctionEventArgs<int> e)
    {
        var page = Math.Max(e.Info, 1);
        LoadPage(page, 10);
    }

    private void LoadPage(int page, int pageSize)
    {
        DataList.Clear();
        foreach (var item in _totalDataList.Skip((page - 1) * pageSize).Take(pageSize))
        {
            DataList.Add(item);
        }
    }

    private static List<DemoDataModel> BuildDataList(int total)
    {
        var list = new List<DemoDataModel>();
        for (var i = 1; i <= total; i++)
        {
            list.Add(new DemoDataModel
            {
                Index = i,
                Name = $"Name{i}",
                IsSelected = i % 2 == 0,
                Type = (DemoType)(i % 6 + 1),
                ImgPath = string.Empty,
                Remark = new string((i % 10).ToString()[0], 10)
            });
        }

        return list;
    }
}
