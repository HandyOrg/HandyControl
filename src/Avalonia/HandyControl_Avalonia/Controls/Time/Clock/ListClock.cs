using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(ElementHourList, typeof(ListBox))]
[TemplatePart(ElementMinuteList, typeof(ListBox))]
[TemplatePart(ElementSecondList, typeof(ListBox))]
public class ListClock : ClockBase
{
    private const string ElementHourList = "PART_HourList";
    private const string ElementMinuteList = "PART_MinuteList";
    private const string ElementSecondList = "PART_SecondList";

    private ListBox? _hourList;
    private ListBox? _minuteList;
    private ListBox? _secondList;

    public override void OnClockOpened() => ScrollIntoView();

    protected override void OnApplyTemplate(Avalonia.Controls.Primitives.TemplateAppliedEventArgs e)
    {
        AppliedTemplate = false;
        if (ButtonConfirm != null)
        {
            ButtonConfirm.Click -= ButtonConfirm_OnClick;
        }
        if (_hourList != null) _hourList.SelectionChanged -= HourList_SelectionChanged;
        if (_minuteList != null) _minuteList.SelectionChanged -= MinuteList_SelectionChanged;
        if (_secondList != null) _secondList.SelectionChanged -= SecondList_SelectionChanged;

        base.OnApplyTemplate(e);

        _hourList = e.NameScope.Find<ListBox>(ElementHourList);
        if (_hourList != null)
        {
            CreateItemsSource(_hourList, 24);
            _hourList.SelectionChanged += HourList_SelectionChanged;
        }

        _minuteList = e.NameScope.Find<ListBox>(ElementMinuteList);
        if (_minuteList != null)
        {
            CreateItemsSource(_minuteList, 60);
            _minuteList.SelectionChanged += MinuteList_SelectionChanged;
        }

        _secondList = e.NameScope.Find<ListBox>(ElementSecondList);
        if (_secondList != null)
        {
            CreateItemsSource(_secondList, 60);
            _secondList.SelectionChanged += SecondList_SelectionChanged;
        }

        ButtonConfirm = e.NameScope.Find<Button>(ElementButtonConfirm);
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

    internal override void Update(DateTime time)
    {
        if (!AppliedTemplate || _hourList == null || _minuteList == null || _secondList == null) return;

        _hourList.SelectedIndex = time.Hour;
        _minuteList.SelectedIndex = time.Minute;
        _secondList.SelectedIndex = time.Second;

        ScrollIntoView();

        DisplayTime = time;
    }

    private void HourList_SelectionChanged(object? sender, SelectionChangedEventArgs e) => UpdateFromSelection();
    private void MinuteList_SelectionChanged(object? sender, SelectionChangedEventArgs e) => UpdateFromSelection();
    private void SecondList_SelectionChanged(object? sender, SelectionChangedEventArgs e) => UpdateFromSelection();

    private static void CreateItemsSource(ItemsControl selector, int count)
    {
        var list = new List<string>(count);
        for (var i = 0; i < count; i++)
        {
            list.Add(i.ToString("00"));
        }
        selector.ItemsSource = list;
    }

    private void UpdateFromSelection()
    {
        if (_hourList == null || _minuteList == null || _secondList == null) return;
        if (_hourList.SelectedIndex >= 0 && _hourList.SelectedIndex < 24 &&
            _minuteList.SelectedIndex >= 0 && _minuteList.SelectedIndex < 60 &&
            _secondList.SelectedIndex >= 0 && _secondList.SelectedIndex < 60)
        {
            var now = DateTime.Now;
            DisplayTime = new DateTime(now.Year, now.Month, now.Day,
                _hourList.SelectedIndex, _minuteList.SelectedIndex, _secondList.SelectedIndex);
        }
    }

    private void ScrollIntoView()
    {
        if (_hourList?.SelectedItem != null) _hourList.ScrollIntoView(_hourList.SelectedItem);
        if (_minuteList?.SelectedItem != null) _minuteList.ScrollIntoView(_minuteList.SelectedItem);
        if (_secondList?.SelectedItem != null) _secondList.ScrollIntoView(_secondList.SelectedItem);
    }
}
