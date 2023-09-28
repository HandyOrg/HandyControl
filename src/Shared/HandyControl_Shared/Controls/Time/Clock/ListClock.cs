using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementHourList, Type = typeof(ListBox))]
[TemplatePart(Name = ElementMinuteList, Type = typeof(ListBox))]
[TemplatePart(Name = ElementSecondList, Type = typeof(ListBox))]
public class ListClock : ClockBase
{
    #region Constants

    private const string ElementHourList = "PART_HourList";
    private const string ElementMinuteList = "PART_MinuteList";
    private const string ElementSecondList = "PART_SecondList";

    #endregion Constants

    #region Data

    private ListBox _hourList;

    private ListBox _minuteList;

    private ListBox _secondList;

    #endregion Data

    public override void OnClockOpened() => ScrollIntoView();

    public override void OnApplyTemplate()
    {
        AppliedTemplate = false;
        if (ButtonConfirm != null)
        {
            ButtonConfirm.Click -= ButtonConfirm_OnClick;
        }

        if (_hourList != null)
        {
            _hourList.SelectionChanged -= HourList_SelectionChanged;
        }

        if (_minuteList != null)
        {
            _minuteList.SelectionChanged -= MinuteList_SelectionChanged;
        }

        if (_secondList != null)
        {
            _secondList.SelectionChanged -= SecondList_SelectionChanged;
        }

        base.OnApplyTemplate();

        _hourList = GetTemplateChild(ElementHourList) as ListBox;
        if (_hourList != null)
        {
            CreateItemsSource(_hourList, 24);
            _hourList.SelectionChanged += HourList_SelectionChanged;
        }

        _minuteList = GetTemplateChild(ElementMinuteList) as ListBox;
        if (_minuteList != null)
        {
            CreateItemsSource(_minuteList, 60);
            _minuteList.SelectionChanged += MinuteList_SelectionChanged;
        }

        _secondList = GetTemplateChild(ElementSecondList) as ListBox;
        if (_secondList != null)
        {
            CreateItemsSource(_secondList, 60);
            _secondList.SelectionChanged += SecondList_SelectionChanged;
        }

        ButtonConfirm = GetTemplateChild(ElementButtonConfirm) as Button;

        if (ButtonConfirm != null)
        {
            ButtonConfirm.Click += ButtonConfirm_OnClick;
        }

        AppliedTemplate = true;
        if (SelectedTime.HasValue)
        {
            Update(SelectedTime.Value);
        }
        else
        {
            DisplayTime = DateTime.Now;
            Update(DisplayTime);
        }
    }

    /// <summary>
    ///     更新
    /// </summary>
    /// <param name="time"></param>
    internal override void Update(DateTime time)
    {
        if (!AppliedTemplate) return;

        var h = time.Hour;
        var m = time.Minute;
        var s = time.Second;

        _hourList.SelectedIndex = h;
        _minuteList.SelectedIndex = m;
        _secondList.SelectedIndex = s;

        ScrollIntoView();

        DisplayTime = time;
    }

    private void HourList_SelectionChanged(object sender, SelectionChangedEventArgs e) => Update();

    private void MinuteList_SelectionChanged(object sender, SelectionChangedEventArgs e) => Update();

    private void SecondList_SelectionChanged(object sender, SelectionChangedEventArgs e) => Update();

    private void CreateItemsSource(ItemsControl selector, int count)
    {
        var list = new List<string>();
        for (var i = 0; i < count; i++)
        {
            list.Add(i.ToString("#00"));
        }

        selector.ItemsSource = list;
    }

    [SuppressMessage("ReSharper", "MergeIntoPattern")]
    private void Update()
    {
        if (_hourList.SelectedIndex >= 0 && _hourList.SelectedIndex < 24 &&
            _minuteList.SelectedIndex >= 0 && _minuteList.SelectedIndex < 60 &&
            _secondList.SelectedIndex >= 0 && _secondList.SelectedIndex < 60)
        {
            var now = DateTime.Now;
            DisplayTime = new DateTime(now.Year, now.Month, now.Day, _hourList.SelectedIndex,
                _minuteList.SelectedIndex, _secondList.SelectedIndex);
        }
    }

    private void ScrollIntoView()
    {
        _hourList.ScrollIntoView(_hourList.SelectedItem);
        _minuteList.ScrollIntoView(_minuteList.SelectedItem);
        _secondList.ScrollIntoView(_secondList.SelectedItem);
    }
}
