using System;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    /// <summary>
    ///     Pagination.xaml 的交互逻辑
    /// </summary>
    public partial class Pagination
    {
        /// <summary>
        ///     页面更新事件
        /// </summary>
        public static readonly RoutedEvent PageUpdatedEvent =
            EventManager.RegisterRoutedEvent("PageUpdated", RoutingStrategy.Bubble,
                typeof(EventHandler<FunctionEventArgs<int>>), typeof(Pagination));

        public Pagination() => InitializeComponent();

        /// <summary>
        ///     最大页数
        /// </summary>
        public static readonly DependencyProperty MaxPageCountProperty = DependencyProperty.Register(
            "MaxPageCount", typeof(int), typeof(Pagination), new PropertyMetadata(1, (o, args) =>
            {
                if (o is Pagination pagination && args.NewValue is int value)
                {
                    if (pagination.PageIndex > pagination.MaxPageCount)
                    {
                        pagination.PageIndex = pagination.MaxPageCount;
                    }
                    pagination.Show(value > 1);
                    pagination.Update();
                }
            }, (o, value) =>
            {
                if (!(o is Pagination)) return 1;
                var intValue = (int)value;
                if (intValue < 1)
                {
                    return 1;
                }
                return intValue;
            }));

        /// <summary>
        ///     最大页数
        /// </summary>
        public int MaxPageCount
        {
            get => (int)GetValue(MaxPageCountProperty);
            set => SetValue(MaxPageCountProperty, value);
        }

        /// <summary>
        ///     每页的数据量
        /// </summary>
        public static readonly DependencyProperty DataCountPerPageProperty = DependencyProperty.Register(
            "DataCountPerPage", typeof(int), typeof(Pagination), new PropertyMetadata(20, (o, args) =>
            {
                if (o is Pagination pagination && args.NewValue is int)
                {
                    pagination.Update();
                }
            }, (o, value) =>
            {
                if (!(o is Pagination)) return 1;
                var intValue = (int)value;
                if (intValue < 1)
                {
                    return 1;
                }
                return intValue;
            }));

        /// <summary>
        ///     每页的数据量
        /// </summary>
        public int DataCountPerPage
        {
            get => (int)GetValue(DataCountPerPageProperty);
            set => SetValue(DataCountPerPageProperty, value);
        }

        /// <summary>
        ///     当前页
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(
            "PageIndex", typeof(int), typeof(Pagination), new PropertyMetadata(1, (o, args) =>
            {
                if (o is Pagination pagination && args.NewValue is int value)
                {
                    pagination.Update();
                    pagination.RaiseEvent(new FunctionEventArgs<int>(PageUpdatedEvent, pagination)
                    {
                        Info = value
                    });
                }
            }, (o, value) =>
            {
                if (!(o is Pagination pagination)) return 1;
                var intValue = (int)value;
                if (intValue < 0)
                {
                    return 0;
                }
                if (intValue > pagination.MaxPageCount) return pagination.MaxPageCount;
                return intValue;
            }));

        /// <summary>
        ///     当前页
        /// </summary>
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        /// <summary>
        ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
        /// </summary>       
        public static readonly DependencyProperty MaxPageIntervalProperty = DependencyProperty.Register(
            "MaxPageInterval", typeof(int), typeof(Pagination), new PropertyMetadata(3, (o, args) =>
            {
                if (o is Pagination pagination)
                {
                    pagination.Update();
                }
            }), value =>
            {
                var intValue = (int)value;
                return intValue >= 0;
            });

        /// <summary>
        ///     表示当前选中的按钮距离左右两个方向按钮的最大间隔（4表示间隔4个按钮，如果超过则用省略号表示）
        /// </summary>   
        public int MaxPageInterval
        {
            get => (int)GetValue(MaxPageIntervalProperty);
            set => SetValue(MaxPageIntervalProperty, value);
        }

        /// <summary>
        ///     页面更新事件
        /// </summary>
        public event EventHandler<FunctionEventArgs<int>> PageUpdated
        {
            add => AddHandler(PageUpdatedEvent, value);
            remove => RemoveHandler(PageUpdatedEvent, value);
        }

        /// <summary>
        ///     更新
        /// </summary>
        private void Update()
        {
            LeftButton.IsEnabled = PageIndex > 1;
            RightButton.IsEnabled = PageIndex < MaxPageCount;
            if (MaxPageInterval == 0)
            {
                FirstButton.Visibility = Visibility.Collapsed;
                LastButton.Visibility = Visibility.Collapsed;
                LeftTextBlock.Visibility = Visibility.Collapsed;
                RightTextBlock.Visibility = Visibility.Collapsed;
                MainPanel.Children.Clear();
                var selectButton = CreateButton(PageIndex);
                MainPanel.Children.Add(selectButton);
                selectButton.IsChecked = true;
                return;
            }
            FirstButton.Visibility = Visibility.Visible;
            LastButton.Visibility = Visibility.Visible;
            LeftTextBlock.Visibility = Visibility.Visible;
            RightTextBlock.Visibility = Visibility.Visible;

            //更新最后一页
            if (MaxPageCount == 1)
            {
                LastButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LastButton.Visibility = Visibility.Visible;
                LastButton.Tag = MaxPageCount.ToString();
            }

            //更新省略号
            var right = MaxPageCount - PageIndex;
            var left = PageIndex - 1;
            RightTextBlock.Visibility = right > MaxPageInterval ? Visibility.Visible : Visibility.Collapsed;
            LeftTextBlock.Visibility = left > MaxPageInterval ? Visibility.Visible : Visibility.Collapsed;

            //更新中间部分
            MainPanel.Children.Clear();
            if (PageIndex > 1 && PageIndex < MaxPageCount)
            {
                var selectButton = CreateButton(PageIndex);
                MainPanel.Children.Add(selectButton);
                selectButton.IsChecked = true;
            }
            else if (PageIndex == 1)
            {
                FirstButton.IsChecked = true;
            }
            else
            {
                LastButton.IsChecked = true;
            }

            var sub = PageIndex;
            for (int i = 0; i < MaxPageInterval - 1; i++)
            {
                if (--sub > 1)
                {
                    MainPanel.Children.Insert(0, CreateButton(sub));
                }
                else
                {
                    break;
                }
            }
            var add = PageIndex;
            for (int i = 0; i < MaxPageInterval - 1; i++)
            {
                if (++add < MaxPageCount)
                {
                    MainPanel.Children.Add(CreateButton(add));
                }
                else
                {
                    break;
                }
            }
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
        {
            PageIndex--;
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            PageIndex++;
        }

        private RadioButton CreateButton(int page)
        {
            return new RadioButton
            {
                Style = FindResource("PaginationButtonStyle") as Style,
                Tag = page.ToString()
            };
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is RadioButton button)) return;
            PageIndex = int.Parse(button.Tag.ToString());
        }
    }
}
