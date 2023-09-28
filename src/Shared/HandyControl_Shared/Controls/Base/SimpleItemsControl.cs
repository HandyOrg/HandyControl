using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using HandyControl.Data;

namespace HandyControl.Controls;

/// <summary>
///     ItemsControl的轻量级版本
/// </summary>
[DefaultProperty("Items")]
[ContentProperty("Items")]
[TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
public class SimpleItemsControl : Control
{
    private const string ElementPanel = "PART_Panel";

    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
        nameof(ItemTemplate), typeof(DataTemplate), typeof(SimpleItemsControl),
        new FrameworkPropertyMetadata(default(DataTemplate), OnItemTemplateChanged));

    public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(
        nameof(ItemContainerStyle), typeof(Style), typeof(SimpleItemsControl),
        new PropertyMetadata(default(Style), OnItemContainerStyleChanged));

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(IEnumerable), typeof(SimpleItemsControl),
        new PropertyMetadata(default(IEnumerable), OnItemsSourceChanged));

    public SimpleItemsControl()
    {
        var items = new ObservableCollection<object>();
        items.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                SetValue(HasItemsPropertyKey, ValueBoxes.TrueBox);
            }
            OnItemsChanged(s, e);
        };
        Items = items;
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable) GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    [Bindable(true)]
    [Category("Content")]
    public Style ItemContainerStyle
    {
        get => (Style) GetValue(ItemContainerStyleProperty);
        set => SetValue(ItemContainerStyleProperty, value);
    }

    [Bindable(true)]
    public DataTemplate ItemTemplate
    {
        get => (DataTemplate) GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Bindable(true)]
    public Collection<object> Items { get; }

    internal Panel ItemsHost { get; set; }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((SimpleItemsControl) d).OnItemsSourceChanged((IEnumerable) e.OldValue, (IEnumerable) e.NewValue);
    }

    protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
    }

    public override void OnApplyTemplate()
    {
        ItemsHost?.Children.Clear();

        base.OnApplyTemplate();

        ItemsHost = GetTemplateChild(ElementPanel) as Panel;
        Refresh();
    }

    protected virtual void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Refresh();
        UpdateItems();
    }

    protected virtual DependencyObject GetContainerForItemOverride() => new ContentPresenter();

    protected virtual bool IsItemItsOwnContainerOverride(object item) => item is ContentPresenter;

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        switch (element)
        {
            case ContentControl contentControl:
                contentControl.Content = item;
                contentControl.ContentTemplate = ItemTemplate;
                break;
            case ContentPresenter contentPresenter:
                contentPresenter.Content = item;
                contentPresenter.ContentTemplate = ItemTemplate;
                break;
        }
    }

    private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => (d as SimpleItemsControl)?.OnItemTemplateChanged(e);

    protected virtual void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e) => Refresh();

    private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => (d as SimpleItemsControl)?.OnItemContainerStyleChanged(e);

    protected virtual void OnItemContainerStyleChanged(DependencyPropertyChangedEventArgs e) => Refresh();

    internal static readonly DependencyPropertyKey HasItemsPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(HasItems), typeof(bool), typeof(SimpleItemsControl),
            new PropertyMetadata(ValueBoxes.FalseBox));

    public static readonly DependencyProperty HasItemsProperty = HasItemsPropertyKey.DependencyProperty;

    public bool HasItems => (bool) GetValue(HasItemsProperty);

    protected virtual void Refresh()
    {
        if (ItemsHost == null) return;

        ItemsHost.Children.Clear();
        foreach (var item in Items)
        {
            DependencyObject container;
            if (IsItemItsOwnContainerOverride(item))
            {
                container = item as DependencyObject;
            }
            else
            {
                container = GetContainerForItemOverride();
                PrepareContainerForItemOverride(container, item);
            }

            if (container is FrameworkElement element)
            {
                element.Style = ItemContainerStyle;
                ItemsHost.Children.Add(element);
            }
        }
    }

    protected virtual void UpdateItems()
    {
    }
}
