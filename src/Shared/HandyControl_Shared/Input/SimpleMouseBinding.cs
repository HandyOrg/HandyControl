using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace HandyControl.Input;

internal sealed class SimpleMouseBinding : InputBinding
{
    public static readonly DependencyProperty MouseActionProperty = DependencyProperty.Register(nameof(MouseAction),
        typeof(MouseAction), typeof(SimpleMouseBinding), new UIPropertyMetadata(MouseAction.None, OnMouseActionPropertyChanged));

    private bool _settingGesture;

    [TypeConverter(typeof(MouseGestureConverter))]
    [ValueSerializer(typeof(MouseGestureValueSerializer))]
    public override InputGesture Gesture
    {
        get => base.Gesture as MouseGesture;
        set
        {
            if (value is not MouseGesture mouseGesture)
            {
                return;
            }

            base.Gesture = mouseGesture;
            SynchronizePropertiesFromGesture(mouseGesture);
        }
    }

    public MouseAction MouseAction
    {
        get => (MouseAction) GetValue(MouseActionProperty);
        set => SetValue(MouseActionProperty, value);
    }

    public SimpleMouseBinding()
    {
    }

    internal SimpleMouseBinding(ICommand command, MouseAction mouseAction) : this(command, new MouseGesture(mouseAction))
    {
    }

    public SimpleMouseBinding(ICommand command, MouseGesture gesture) : base(command, gesture)
    {
        SynchronizePropertiesFromGesture(gesture);
    }

    private static void OnMouseActionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((SimpleMouseBinding) d).SynchronizeGestureFromProperties((MouseAction) e.NewValue);
    }

    protected override Freezable CreateInstanceCore()
    {
        return new SimpleMouseBinding();
    }

    private void SynchronizePropertiesFromGesture(MouseGesture mouseGesture)
    {
        if (_settingGesture)
        {
            return;
        }

        _settingGesture = true;

        try
        {
            MouseAction = mouseGesture.MouseAction;
        }
        finally
        {
            _settingGesture = false;
        }
    }

    private void SynchronizeGestureFromProperties(MouseAction mouseAction)
    {
        if (_settingGesture)
        {
            return;
        }

        _settingGesture = true;

        try
        {
            if (Gesture is null)
            {
                Gesture = new MouseGesture(mouseAction);
            }
            else
            {
                ((MouseGesture) Gesture).MouseAction = mouseAction;
            }
        }
        finally
        {
            _settingGesture = false;
        }
    }
}
