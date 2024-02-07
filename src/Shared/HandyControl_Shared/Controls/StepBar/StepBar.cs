using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;

namespace HandyControl.Controls;

[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(StepBarItem))]
[DefaultEvent("StepChanged")]
[TemplatePart(Name = ElementProgressBarBack, Type = typeof(ProgressBar))]
public class StepBar : ItemsControl
{
    private const string ElementProgressBarBack = "PART_ProgressBarBack";

    private ProgressBar _progressBarBack;
    private int _oriStepIndex = -1;

    public StepBar()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Next, (_, _) => Next()));
        CommandBindings.Add(new CommandBinding(ControlCommands.Prev, (_, _) => Prev()));

        ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        AddHandler(SelectableItem.SelectedEvent, new RoutedEventHandler(OnStepBarItemSelected));
    }

    private void OnStepBarItemSelected(object sender, RoutedEventArgs e)
    {
        if (!IsMouseSelectable)
        {
            return;
        }

        if (e.OriginalSource is StepBarItem item)
        {
            SetCurrentValue(StepIndexProperty, item.Index - 1);
        }
    }

    private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
    {
        if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        {
            return;
        }

        int count = Items.Count;
        InvalidateVisual();
        _progressBarBack.Maximum = count - 1;
        _progressBarBack.Value = StepIndex;

        if (count <= 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            if (ItemContainerGenerator.ContainerFromIndex(i) is StepBarItem stepBarItem)
            {
                stepBarItem.Index = i + 1;
            }
        }

        if (_oriStepIndex > 0)
        {
            StepIndex = _oriStepIndex;
            _oriStepIndex = -1;
        }
        else
        {
            OnStepIndexChanged(StepIndex);
        }
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is StepBarItem;

    protected override DependencyObject GetContainerForItemOverride() => new StepBarItem();

    /// <summary>
    ///     步骤改变事件
    /// </summary>
    public static readonly RoutedEvent StepChangedEvent =
        EventManager.RegisterRoutedEvent("StepChanged", RoutingStrategy.Bubble,
            typeof(EventHandler<FunctionEventArgs<int>>), typeof(StepBar));

    /// <summary>
    ///     步骤改变事件
    /// </summary>
    [Category("Behavior")]
    public event EventHandler<FunctionEventArgs<int>> StepChanged
    {
        add => AddHandler(StepChangedEvent, value);
        remove => RemoveHandler(StepChangedEvent, value);
    }

    public static readonly DependencyProperty StepIndexProperty = DependencyProperty.Register(
        nameof(StepIndex), typeof(int), typeof(StepBar), new FrameworkPropertyMetadata(ValueBoxes.Int0Box,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStepIndexChanged, CoerceStepIndex));

    private static object CoerceStepIndex(DependencyObject d, object baseValue)
    {
        var ctl = (StepBar) d;
        int stepIndex = (int) baseValue;
        if (ctl.Items.Count == 0 && stepIndex > 0)
        {
            ctl._oriStepIndex = stepIndex;
            return ValueBoxes.Int0Box;
        }

        return stepIndex < 0
            ? ValueBoxes.Int0Box
            : stepIndex >= ctl.Items.Count
                ? ctl.Items.Count == 0 ? 0 : ctl.Items.Count - 1
                : baseValue;
    }

    private static void OnStepIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (StepBar) d;
        ctl.OnStepIndexChanged((int) e.NewValue);
    }

    private void OnStepIndexChanged(int stepIndex)
    {
        for (int i = 0; i < stepIndex; i++)
        {
            if (ItemContainerGenerator.ContainerFromIndex(i) is StepBarItem stepItemFinished)
            {
                stepItemFinished.Status = StepStatus.Complete;
            }
        }

        for (int i = stepIndex + 1; i < Items.Count; i++)
        {
            if (ItemContainerGenerator.ContainerFromIndex(i) is StepBarItem stepItemFinished)
            {
                stepItemFinished.Status = StepStatus.Waiting;
            }
        }

        if (ItemContainerGenerator.ContainerFromIndex(stepIndex) is StepBarItem stepItemSelected)
        {
            stepItemSelected.Status = StepStatus.UnderWay;
        }

        _progressBarBack?.BeginAnimation(RangeBase.ValueProperty, AnimationHelper.CreateAnimation(stepIndex));

        RaiseEvent(new FunctionEventArgs<int>(StepChangedEvent, this)
        {
            Info = stepIndex
        });
    }

    public int StepIndex
    {
        get => (int) GetValue(StepIndexProperty);
        set => SetValue(StepIndexProperty, value);
    }

    public static readonly DependencyProperty DockProperty = DependencyProperty.Register(
        nameof(Dock), typeof(Dock), typeof(StepBar), new PropertyMetadata(Dock.Top));

    public Dock Dock
    {
        get => (Dock) GetValue(DockProperty);
        set => SetValue(DockProperty, value);
    }

    public static readonly DependencyProperty IsMouseSelectableProperty = DependencyProperty.Register(
        nameof(IsMouseSelectable), typeof(bool), typeof(StepBar), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsMouseSelectable
    {
        get => (bool) GetValue(IsMouseSelectableProperty);
        set => SetValue(IsMouseSelectableProperty, ValueBoxes.BooleanBox(value));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _progressBarBack = GetTemplateChild(ElementProgressBarBack) as ProgressBar;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        int colCount = Items.Count;
        if (_progressBarBack == null || colCount <= 0)
        {
            return;
        }

        if (Dock is Dock.Top or Dock.Bottom)
        {
            _progressBarBack.Width = (colCount - 1) * (ActualWidth / colCount);
        }
        else
        {
            _progressBarBack.Height = (colCount - 1) * (ActualHeight / colCount);
        }
    }

    public void Next() => StepIndex++;

    public void Prev() => StepIndex--;
}
