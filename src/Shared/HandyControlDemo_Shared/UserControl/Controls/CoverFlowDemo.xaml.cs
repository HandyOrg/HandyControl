using System;

namespace HandyControlDemo.UserControl;

public partial class CoverFlowDemo
{
    public CoverFlowDemo()
    {
        InitializeComponent();

        CoverFlowMain.AddRange(new[]
        {
            new Uri(@"pack://application:,,,/Resources/Img/Album/1.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/2.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/3.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/4.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/5.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/6.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/7.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/8.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/9.jpg"),
            new Uri(@"pack://application:,,,/Resources/Img/Album/10.jpg")
        });

        CoverFlowMain.PageIndex = 2;
    }
}
