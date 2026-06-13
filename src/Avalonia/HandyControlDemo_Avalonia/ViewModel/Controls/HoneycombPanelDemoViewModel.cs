using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HandyControlDemo.ViewModel;

public partial class HoneycombPanelDemoViewModel : ObservableObject
{
    private static readonly Random Random = new();

    [ObservableProperty] private ObservableCollection<Bitmap> _dataList = [];

    public HoneycombPanelDemoViewModel()
    {
        for (int i = 0; i < 7; i++)
        {
            DataList.Add(LoadRandomAvatar());
        }
    }

    [RelayCommand]
    private void AddItem() => DataList.Insert(0, LoadRandomAvatar());

    [RelayCommand]
    private void RemoveItem()
    {
        if (DataList.Count > 0)
        {
            DataList.RemoveAt(0);
        }
    }

    private static Bitmap LoadRandomAvatar()
    {
        var uri = new Uri($"avares://HandyControlDemo/Resources/Img/Avatar/avatar{Random.Next(1, 7)}.png");
        using var stream = AssetLoader.Open(uri);
        return new Bitmap(stream);
    }
}
