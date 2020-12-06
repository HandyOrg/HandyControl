﻿using System.Windows;
using System.Windows.Media.Imaging;

namespace HandyControlDemo.UserControl
{
    public partial class ButtonGroupDemoCtl
    {
        public ButtonGroupDemoCtl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(BitmapFrame), typeof(Avatar), new PropertyMetadata(default(BitmapFrame)));

        public BitmapFrame Source
        {
            get => (BitmapFrame) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            "DisplayName", typeof(string), typeof(Avatar), new PropertyMetadata(default(string)));

        public string DisplayName
        {
            get => (string) GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public static readonly DependencyProperty LinkProperty = DependencyProperty.Register(
            "Link", typeof(string), typeof(Avatar), new PropertyMetadata(default(string)));

        public string Link
        {
            get => (string) GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }
    }
}
