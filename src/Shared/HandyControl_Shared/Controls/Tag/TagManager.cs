using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HandyControl.Controls
{
    public class TagManager
    {
        internal static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }
            UIElement uIElement = d as UIElement;
            if (uIElement != null)
            {
                uIElement.AddHandler(routedEvent, handler);
            }
            else
            {
                ContentElement contentElement = d as ContentElement;
                if (contentElement != null)
                {
                    contentElement.AddHandler(routedEvent, handler);
                }
                else
                {
                    UIElement3D uIElement3D = d as UIElement3D;
                    if (uIElement3D == null)
                    {
                        throw new ArgumentException("Invalid_IInputElement");
                    }
                    uIElement3D.AddHandler(routedEvent, handler);
                }
            }
        }

        internal static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }
            UIElement uIElement = d as UIElement;
            if (uIElement != null)
            {
                uIElement.RemoveHandler(routedEvent, handler);
            }
            else
            {
                ContentElement contentElement = d as ContentElement;
                if (contentElement != null)
                {
                    contentElement.RemoveHandler(routedEvent, handler);
                }
                else
                {
                    UIElement3D uIElement3D = d as UIElement3D;
                    if (uIElement3D == null)
                    {
                        throw new ArgumentException("Invalid_IInputElement");
                    }
                    uIElement3D.RemoveHandler(routedEvent, handler);
                }
            }
        }

        public static void AddTagClosedEventHandler(DependencyObject element, RoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            AddHandler(element, Tag.ClosedEvent, handler);
        }

        public static void RemoveTagClosedEventHandler(DependencyObject element, RoutedEventHandler handler)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            RemoveHandler(element, Tag.ClosedEvent, handler);
        }



        public static bool GetCanCloseTag(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanCloseTagProperty);
        }


        public static void SetCanCloseTag(DependencyObject obj, bool value)
        {
            obj.SetValue(CanCloseTagProperty, value);
        }

        public static void OnCanCloseTagPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool flag)
            {
                if (flag)
                {
                    AddTagClosedEventHandler(obj, DefaultTagClosed);
                }
                else
                {
                    RemoveTagClosedEventHandler(obj, DefaultTagClosed);
                }
            }
        }

        public static readonly DependencyProperty CanCloseTagProperty =
            DependencyProperty.RegisterAttached("ShadowCanCloseTag", typeof(bool), typeof(TagManager), new PropertyMetadata(false,new PropertyChangedCallback(OnCanCloseTagPropertyChanged)));


        private static void DefaultTagClosed(object sender, RoutedEventArgs e)
        {
            if (sender is TagPanel panel)
            {
                panel.Children.Remove(e.OriginalSource as Tag);
            }
        }

    }
}
