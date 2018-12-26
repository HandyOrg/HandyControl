using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class Rate : ItemsControl
    {
        private bool _isLoaded;

        public Rate()
        {
            AddHandler(RateItem.SelectedChangedEvent, new RoutedEventHandler(RateItemSelectedChanged));
            AddHandler(RateItem.ValueChangedEvent, new RoutedEventHandler(RateItemValueChanged));

            Loaded += (s, e) =>
            {
                if (_isLoaded) return;
                _isLoaded = true;
                if (Value <= 0)
                {
                    if (DefaultValue > 0)
                    {
                        Value = DefaultValue;
                        UpdateItems();
                    }
                }
                else
                {
                    UpdateItems();
                }
            };
        }

        private void RateItemValueChanged(object sender, RoutedEventArgs e) => Value =
            (from RateItem item in Items where item.IsSelected select item.IsHalf ? 0.5 : 1).Sum();

        private void RateItemSelectedChanged(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is RateItem rateItem)
            {
                var index = rateItem.Index;
                for (int i = 0; i < index; i++)
                {
                    if (Items[i] is RateItem item)
                    {
                        item.IsSelected = true;
                        item.IsHalf = false;
                    }
                }
                for (int i = index; i < Count; i++)
                {
                    if (Items[i] is RateItem item)
                    {
                        item.IsSelected = false;
                        item.IsHalf = false;
                    }
                }
            }
        }

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            "ItemMargin", typeof(Thickness), typeof(Rate), new PropertyMetadata(default(Thickness)));

        public Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
            "ItemWidth", typeof(double), typeof(Rate), new PropertyMetadata(ValueBoxes.Double20Box));

        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
            "ItemHeight", typeof(double), typeof(Rate), new PropertyMetadata(ValueBoxes.Double20Box));

        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public static readonly DependencyProperty AllowHalfProperty = DependencyProperty.Register(
            "AllowHalf", typeof(bool), typeof(Rate), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool AllowHalf
        {
            get => (bool)GetValue(AllowHalfProperty);
            set => SetValue(AllowHalfProperty, value);
        }

        public static readonly DependencyProperty AllowClearProperty = DependencyProperty.Register(
            "AllowClear", typeof(bool), typeof(Rate), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool AllowClear
        {
            get => (bool)GetValue(AllowClearProperty);
            set => SetValue(AllowClearProperty, value);
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(Geometry), typeof(Rate), new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
            "Count", typeof(int), typeof(Rate), new PropertyMetadata(ValueBoxes.Int5Box));

        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            "DefaultValue", typeof(double), typeof(Rate), new PropertyMetadata(ValueBoxes.Double0Box));

        public double DefaultValue
        {
            get => (double)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(Rate), new PropertyMetadata(ValueBoxes.Double0Box));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Rate), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            "ShowText", typeof(bool), typeof(Rate), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool ShowText
        {
            get => (bool)GetValue(ShowTextProperty);
            set => SetValue(ShowTextProperty, value);
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is RateItem;

        protected override DependencyObject GetContainerForItemOverride() => new RateItem();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!_isLoaded)
            {
                Items.Clear();

                for (int i = 1; i <= Count; i++)
                {
                    Items.Add(new RateItem
                    {
                        Index = i
                    });
                }
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            UpdateItems();
        }

        private void UpdateItems()
        {
            var count = (int)Value;
            for (int i = 0; i < count; i++)
            {
                if (Items[i] is RateItem rateItem)
                {
                    rateItem.IsSelected = true;
                    rateItem.IsHalf = false;
                }
            }
            if (Value > count)
            {
                if (Items[count] is RateItem rateItem)
                {
                    rateItem.IsSelected = true;
                    rateItem.IsHalf = true;
                }
                count += 1;
            }
            for (int i = count; i < Count; i++)
            {
                if (Items[i] is RateItem rateItem)
                {
                    rateItem.IsSelected = false;
                    rateItem.IsHalf = false;
                }
            }
        }

        public void Reset() => Value = DefaultValue;
    }
}
