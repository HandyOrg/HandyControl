using System;
using System.Windows;
using HandyControl.Controls;
using HandyControlDemo.Data;

namespace HandyControlDemo.Window
{
    public partial class DialogDemoWindow
    {
        public DialogDemoWindow()
        {
            InitializeComponent();

            var dialogToken = $"{MessageToken.DialogDemoWindow}+{DateTime.Now:yyyyMMddHHmmssfff}";
            DialogToken = dialogToken;
            Dialog.SetToken(this, dialogToken);
        }

        public static readonly DependencyProperty DialogTokenProperty = DependencyProperty.Register(
            "DialogToken", typeof(string), typeof(DialogDemoWindow), new PropertyMetadata(default(string)));

        public string DialogToken
        {
            get => (string) GetValue(DialogTokenProperty);
            set => SetValue(DialogTokenProperty, value);
        }
    }
}
