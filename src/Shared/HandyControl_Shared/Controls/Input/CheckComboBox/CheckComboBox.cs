using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Interactivity;

namespace HandyControl.Controls;

[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
[TemplatePart(Name = ElementSelectAll, Type = typeof(CheckComboBoxItem))]
public class CheckComboBox : ListBox
{
    private const string ElementPanel = "PART_Panel";

    private const string ElementSelectAll = "PART_SelectAll";

    private Panel _panel;

    private CheckComboBoxItem _selectAllItem;

    private bool _isInternalAction;

    public static readonly DependencyProperty MaxDropDownHeightProperty =
        System.Windows.Controls.ComboBox.MaxDropDownHeightProperty.AddOwner(typeof(CheckComboBox),
            new FrameworkPropertyMetadata(SystemParameters.PrimaryScreenHeight / 3));

    [Bindable(true), Category("Layout")]
    [TypeConverter(typeof(LengthConverter))]
    public double MaxDropDownHeight
    {
        get => (double) GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }

    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen), typeof(bool), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox, OnIsDropDownOpenChanged, CoerceIsDropDownOpen));

    private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (CheckComboBox) d;

        if (!(bool) e.NewValue)
        {
            ctl.Dispatcher.BeginInvoke(new Action(() =>
            {
                Mouse.Capture(null);
            }), DispatcherPriority.Send);
        }
    }

    private static object CoerceIsDropDownOpen(DependencyObject d, object baseValue)
    {
        if (((CheckComboBox) d).IsReadOnly)
        {
            return ValueBoxes.FalseBox;
        }

        return baseValue;
    }

    public bool IsDropDownOpen
    {
        get => (bool) GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty TagStyleProperty = DependencyProperty.Register(
        nameof(TagStyle), typeof(Style), typeof(CheckComboBox), new PropertyMetadata(default(Style)));

    public Style TagStyle
    {
        get => (Style) GetValue(TagStyleProperty);
        set => SetValue(TagStyleProperty, value);
    }

    public static readonly DependencyProperty TagSpacingProperty = DependencyProperty.Register(
        nameof(TagSpacing), typeof(double), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.Double0Box));

    public double TagSpacing
    {
        get => (double) GetValue(TagSpacingProperty);
        set => SetValue(TagSpacingProperty, value);
    }

    public static readonly DependencyProperty ShowSelectAllButtonProperty = DependencyProperty.Register(
        nameof(ShowSelectAllButton), typeof(bool), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool ShowSelectAllButton
    {
        get => (bool) GetValue(ShowSelectAllButtonProperty);
        set => SetValue(ShowSelectAllButtonProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly), typeof(bool), typeof(CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsReadOnly
    {
        get => (bool) GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
    }

    public CheckComboBox()
    {
        AddHandler(Controls.Tag.ClosedEvent, new RoutedEventHandler(Tags_OnClosed));

        CommandBindings.Add(new CommandBinding(ControlCommands.Clear, (s, e) =>
        {
            SetCurrentValue(SelectedValueProperty, null);
            SetCurrentValue(SelectedItemProperty, null);
            SetCurrentValue(SelectedIndexProperty, -1);
            SelectedItems.Clear();
        }, (s, e) => e.CanExecute = !IsReadOnly));
    }

    public override void OnApplyTemplate()
    {
        if (_selectAllItem != null)
        {
            _selectAllItem.Selected -= SelectAllItem_Selected;
            _selectAllItem.Unselected -= SelectAllItem_Unselected;
        }

        base.OnApplyTemplate();

        _panel = GetTemplateChild(ElementPanel) as Panel;
        _selectAllItem = GetTemplateChild(ElementSelectAll) as CheckComboBoxItem;
        if (_selectAllItem != null)
        {
            _selectAllItem.Selected += SelectAllItem_Selected;
            _selectAllItem.Unselected += SelectAllItem_Unselected;
        }

        UpdateTags();
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        UpdateTags();

        base.OnSelectionChanged(e);
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is CheckComboBoxItem;

    protected override DependencyObject GetContainerForItemOverride() => new CheckComboBoxItem();

    protected override void OnDisplayMemberPathChanged(string oldDisplayMemberPath, string newDisplayMemberPath) => UpdateTags();

    private void Tags_OnClosed(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Tag tag)
        {
            SelectedItems.Remove(tag.Tag);
            _panel.Children.Remove(tag);
        }
    }

    private void SwitchAllItems(bool selected)
    {
        if (_isInternalAction) return;
        _isInternalAction = true;

        if (!selected)
        {
            UnselectAll();
        }
        else
        {
            SelectAll();
        }

        _isInternalAction = false;
        UpdateTags();
    }

    private void SelectAllItem_Selected(object sender, RoutedEventArgs e) => SwitchAllItems(true);

    private void SelectAllItem_Unselected(object sender, RoutedEventArgs e) => SwitchAllItems(false);

    private void UpdateTags()
    {
        if (_panel == null || _isInternalAction) return;

        if (_selectAllItem != null)
        {
            _isInternalAction = true;
            _selectAllItem.SetCurrentValue(IsSelectedProperty, Items.Count > 0 && SelectedItems.Count == Items.Count);
            _isInternalAction = false;
        }

        _panel.Children.Clear();
        var tagStyle = TagStyle;
        var isReadOnly = IsReadOnly;
        var displayMemberPath = DisplayMemberPath;

        foreach (var item in SelectedItems)
        {
            var tag = new Tag
            {
                Style = tagStyle,
                Tag = item,
                ShowCloseButton = !isReadOnly
            };

            if (ItemsSource != null)
            {
                tag.SetBinding(ContentControl.ContentProperty, new Binding(displayMemberPath) { Source = item });
            }
            else
            {
                tag.Content = IsItemItsOwnContainerOverride(item) ? ((CheckComboBoxItem) item).Content : item;
            }

            _panel.Children.Add(tag);
        }
    }
}
