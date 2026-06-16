using System.Collections.ObjectModel;
using Avalonia.Interactivity;

namespace HandyControlDemo.UserControl;

public class TagDemoItem : Avalonia.AvaloniaObject
{
    public static readonly Avalonia.StyledProperty<string?> NameProperty =
        Avalonia.AvaloniaProperty.Register<TagDemoItem, string?>(nameof(Name));

    public static readonly Avalonia.StyledProperty<bool> IsSelectedProperty =
        Avalonia.AvaloniaProperty.Register<TagDemoItem, bool>(nameof(IsSelected));

    public string? Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
}

public partial class TagDemo : Avalonia.Controls.UserControl
{
    public ObservableCollection<TagDemoItem> DataList { get; } = new();

    public TagDemo()
    {
        InitializeComponent();

        for (var i = 1; i <= 10; i++)
        {
            DataList.Add(new TagDemoItem
            {
                Name = $"Name{i}",
                IsSelected = i % 2 == 0
            });
        }

        BoundContainer.ItemsSource = DataList;
        AddButton.Click += AddButton_Click;
    }

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        var name = TagNameBox.Text;
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        DataList.Insert(0, new TagDemoItem
        {
            Name = name,
            IsSelected = DataList.Count % 2 == 0
        });
        TagNameBox.Text = string.Empty;
    }
}
