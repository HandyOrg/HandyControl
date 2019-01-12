using System.Windows;
using System.Windows.Input;

namespace HandyControl.Controls
{
    public class ElementBase
    {
        /// <summary>
        /// 注册属性
        /// public static readonly DependencyProperty Property = Utility.Property<T,T>(string,T);
        /// </summary>
        /// <typeparam name="thisType">this</typeparam>
        /// <typeparam name="propertyType">如：string，bool</typeparam>
        /// <param name="name">属性名</param>
        /// <param name="defaultValue">属性值</param>
        /// <returns></returns>
        public static DependencyProperty Property<thisType, propertyType>(string name, propertyType defaultValue)
        {
            return DependencyProperty.Register(name.Replace("Property", ""), typeof(propertyType), typeof(thisType), new PropertyMetadata(defaultValue));
        }

        /// <summary>
        /// 注册属性
        /// public static readonly DependencyProperty Property = Utility.Property<T,T>(string,T);
        /// </summary>
        /// <typeparam name="thisType"></typeparam>
        /// <typeparam name="propertyType"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DependencyProperty Property<thisType, propertyType>(string name)
        {
            return DependencyProperty.Register(name.Replace("Property", ""), typeof(propertyType), typeof(thisType));
        }

        /// <summary>
        /// 注册事件
        /// public static readonly RoutedEvent NameRoutedEvent = Utility.RoutedEvent<T,T>(string,T);
        /// public event EventHandler Name { add { AddHandler(EventHandler, value); } remove { RemoveHandler(EventHandler, value); } }
        /// </summary>
        /// <typeparam name="thisType"></typeparam>
        /// <typeparam name="propertyType"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RoutedEvent RoutedEvent<thisType, propertyType>(string name)
        {
            return EventManager.RegisterRoutedEvent(name.Replace("Event", ""), RoutingStrategy.Bubble, typeof(propertyType), typeof(thisType));
        }

        /// <summary>
        /// 默认样式
        /// Utility.DefaultStyle<T>(DefaultStyleKeyProperty);
        /// </summary>
        /// <typeparam name="thisType">this</typeparam>
        /// <param name="dp">DefaultStyleKeyProperty</param>
        public static void DefaultStyle<thisType>(DependencyProperty dp)
        {
            dp.OverrideMetadata(typeof(thisType), new FrameworkPropertyMetadata(typeof(thisType)));
        }

        /// <summary>
        /// 初始化一个 Command
        /// </summary>
        /// <typeparam name="thisType"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RoutedUICommand Command<thisType>(string name)
        {
            return new RoutedUICommand(name, name, typeof(thisType));
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="element"></param>
        /// <param name="state"></param>
        public static string GoToState(FrameworkElement element, string state)
        {
            VisualStateManager.GoToState(element, state, false);
            return state;
        }
    }
}
