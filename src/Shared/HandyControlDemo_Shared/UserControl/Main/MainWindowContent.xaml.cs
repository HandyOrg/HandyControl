using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace HandyControlDemo.UserControl;

public partial class MainWindowContent
{
    private GridLength _columnDefinitionWidth;

    public MainWindowContent()
    {
        InitializeComponent();
    }

    private void OnLeftMainContentShiftOut(object sender, RoutedEventArgs e)
    {
        ButtonShiftOut.Collapse();
        GridSplitter.IsEnabled = false;

        double targetValue = -ColumnDefinitionLeft.MaxWidth;
        _columnDefinitionWidth = ColumnDefinitionLeft.Width;

        DoubleAnimation animation = AnimationHelper.CreateAnimation(targetValue, milliseconds: 100);
        animation.FillBehavior = FillBehavior.Stop;
        animation.Completed += OnAnimationCompleted;
        LeftMainContent.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);

        void OnAnimationCompleted(object _, EventArgs args)
        {
            animation.Completed -= OnAnimationCompleted;
            LeftMainContent.RenderTransform.SetCurrentValue(TranslateTransform.XProperty, targetValue);

            Grid.SetColumn(MainContent, 0);
            Grid.SetColumnSpan(MainContent, 2);

            ColumnDefinitionLeft.MinWidth = 0;
            ColumnDefinitionLeft.Width = new GridLength();
            ButtonShiftIn.Show();
        }
    }

    private void OnLeftMainContentShiftIn(object sender, RoutedEventArgs e)
    {
        ButtonShiftIn.Collapse();
        GridSplitter.IsEnabled = true;

        double targetValue = ColumnDefinitionLeft.Width.Value;

        DoubleAnimation animation = AnimationHelper.CreateAnimation(targetValue, milliseconds: 100);
        animation.FillBehavior = FillBehavior.Stop;
        animation.Completed += OnAnimationCompleted;
        LeftMainContent.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);

        void OnAnimationCompleted(object _, EventArgs args)
        {
            animation.Completed -= OnAnimationCompleted;
            LeftMainContent.RenderTransform.SetCurrentValue(TranslateTransform.XProperty, targetValue);

            Grid.SetColumn(MainContent, 1);
            Grid.SetColumnSpan(MainContent, 1);

            ColumnDefinitionLeft.MinWidth = 240;
            ColumnDefinitionLeft.Width = _columnDefinitionWidth;
            ButtonShiftOut.Show();
        }
    }
}
