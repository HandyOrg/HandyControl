using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class ChatBubble : ContentControl
    {
        public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(
            "Role", typeof(ChatRoleType), typeof(ChatBubble), new PropertyMetadata(default(ChatRoleType)));

        public ChatRoleType Role
        {
            get => (ChatRoleType) GetValue(RoleProperty);
            set => SetValue(RoleProperty, value);
        }
    }
}