using System;
using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class ChatBubble : SelectableItem
    {
        public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(
            "Role", typeof(ChatRoleType), typeof(ChatBubble), new PropertyMetadata(default(ChatRoleType)));

        public ChatRoleType Role
        {
            get => (ChatRoleType) GetValue(RoleProperty);
            set => SetValue(RoleProperty, value);
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(ChatMessageType), typeof(ChatBubble), new PropertyMetadata(default(ChatMessageType)));

        public ChatMessageType Type
        {
            get => (ChatMessageType) GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty IsReadProperty = DependencyProperty.Register(
            "IsRead", typeof(bool), typeof(ChatBubble), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsRead
        {
            get => (bool) GetValue(IsReadProperty);
            set => SetValue(IsReadProperty, ValueBoxes.BooleanBox(value));
        }

        public static void SetMaxWidth(DependencyObject element, double value)
            => element.SetValue(MaxWidthProperty, value);

        public static double GetMaxWidth(DependencyObject element)
            => (double) element.GetValue(MaxWidthProperty);

        public Action<object> ReadAction { get; set; }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);

            IsRead = true;
            ReadAction?.Invoke(Content);
        }
    }
}