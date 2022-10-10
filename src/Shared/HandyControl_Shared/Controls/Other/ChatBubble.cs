using System;
using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls;

public class ChatBubble : SelectableItem
{
    public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(
        nameof(Role), typeof(ChatRoleType), typeof(ChatBubble), new PropertyMetadata(default(ChatRoleType)));

    public ChatRoleType Role
    {
        get => (ChatRoleType) GetValue(RoleProperty);
        set => SetValue(RoleProperty, value);
    }

    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
        nameof(Type), typeof(ChatMessageType), typeof(ChatBubble), new PropertyMetadata(default(ChatMessageType)));

    public ChatMessageType Type
    {
        get => (ChatMessageType) GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public static readonly DependencyProperty IsReadProperty = DependencyProperty.Register(
        nameof(IsRead), typeof(bool), typeof(ChatBubble), new PropertyMetadata(ValueBoxes.FalseBox));

    public bool IsRead
    {
        get => (bool) GetValue(IsReadProperty);
        set => SetValue(IsReadProperty, ValueBoxes.BooleanBox(value));
    }

    public Action<object> ReadAction { get; set; }

    protected override void OnSelected(RoutedEventArgs e)
    {
        base.OnSelected(e);

        IsRead = true;
        ReadAction?.Invoke(Content);
    }
}
