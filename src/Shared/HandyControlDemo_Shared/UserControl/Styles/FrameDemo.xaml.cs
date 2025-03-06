using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HandyControl.Tools.Extension;

namespace HandyControlDemo.UserControl;

public partial class FrameDemo
{

    private readonly List<Page> _pageList;

    public FrameDemo()
    {
        InitializeComponent();

        AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Button_Click));

        _pageList = new List<Page>();
        for (var i = 0; i < 5; i++)
        {
            _pageList.Add(CreatePage(i));
        }

        frameDemo.Navigate(_pageList[0]);
    }

    private Page CreatePage(int index)
    {
        var indexStr = index.ToString();
        var button = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Content = indexStr,
            Width = 320,
            Tag = indexStr
        };

        return new Page
        {
            Title = indexStr,
            Content = button
        };
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Button button && button.Tag is string tag)
        {
            var index = tag.Value<int>() + 1;
            frameDemo.Navigate(index >= _pageList.Count ? _pageList[0] : _pageList[index]);
        }
    }
}
