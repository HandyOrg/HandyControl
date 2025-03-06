using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using HandyControl.Tools;
using HandyControlDemo.Data;

namespace HandyControlDemo.UserControl;

public partial class GeometryDemo
{
    private readonly HashSet<string> _lineSet = new()
    {
        "CheckedGeometry"
    };

    public ObservableCollection<GeometryItemModel> GeometryItems { get; set; } =
        new();

    public GeometryDemo()
    {
        InitializeComponent();
        GenerateGeometries();
    }

    public void GenerateGeometries()
    {
        foreach (var key in Application.Current.Resources.MergedDictionaries[1].MergedDictionaries[0].Keys.OfType<string>().OrderBy(item => item))
        {
            if (!key.EndsWith("Geometry")) continue;
            GeometryItems.Add(new GeometryItemModel
            {
                Key = key,
                Data = ResourceHelper.GetResource<Geometry>(key),
                Line = _lineSet.Contains(key)
            });
        }
    }
}
