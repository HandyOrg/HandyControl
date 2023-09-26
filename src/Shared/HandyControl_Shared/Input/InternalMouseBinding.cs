using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace HandyControl.Input;

internal class InternalMouseBinding : InputBinding
{
    public static readonly DependencyProperty MouseActionProperty = DependencyProperty.Register(nameof(MouseAction),
        typeof(MouseAction), typeof(MouseBinding), new UIPropertyMetadata(MouseAction.None, new PropertyChangedCallback(OnMouseActionPropertyChanged)));

    private bool _settingGesture = false;

    [TypeConverter(typeof(MouseGestureConverter))]
    [ValueSerializer(typeof(MouseGestureValueSerializer))]
    public override InputGesture Gesture
    {
        get => base.Gesture as MouseGesture;
        set
        {
            if (value is MouseGesture mouseGesture)
            {
                base.Gesture = mouseGesture;
                SynchronizePropertiesFromGesture(mouseGesture);
            }
        }
    }

    public MouseAction MouseAction
    {
        get => (MouseAction) GetValue(MouseActionProperty);
        set => SetValue(MouseActionProperty, value);
    }

    public InternalMouseBinding()
    {
    }

    internal InternalMouseBinding(ICommand command, MouseAction mouseAction) : this(command, new MouseGesture(mouseAction))
    {
    }

    public InternalMouseBinding(ICommand command, MouseGesture gesture) : base(command, gesture)
    {
        SynchronizePropertiesFromGesture(gesture);
    }

    private static void OnMouseActionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((InternalMouseBinding) d).SynchronizeGestureFromProperties((MouseAction) e.NewValue);
    }

    protected override Freezable CreateInstanceCore()
    {
        return new InternalMouseBinding();
    }

    private void SynchronizePropertiesFromGesture(MouseGesture mouseGesture)
    {
        if (!_settingGesture)
        {
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
    }

    private void SynchronizeGestureFromProperties(MouseAction mouseAction)
    {
        if (!_settingGesture)
        {
            _settingGesture = true;
            try
            {
                if (Gesture == null)
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

    private void OnMouseGesturePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (string.Compare(e.PropertyName, "MouseAction", StringComparison.Ordinal) == 0 &&
            Gesture is MouseGesture mouseGesture)
        {
            SynchronizePropertiesFromGesture(mouseGesture);
        }
    }
}
