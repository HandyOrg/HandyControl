using System.Windows;
using HandyControl.Data;

namespace HandyControl.Controls
{
    public class PasswordBoxMonitor
    {
        /// <summary>
        ///     密码长度
        /// </summary>
        public static readonly DependencyProperty PasswordLengthProperty = DependencyProperty.RegisterAttached(
            "PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new PropertyMetadata(default(int)));

        public static void SetPasswordLength(DependencyObject element, int value) => element.SetValue(PasswordLengthProperty, value);

        public static int GetPasswordLength(DependencyObject element) => (int)element.GetValue(PasswordLengthProperty);

        /// <summary>
        ///     是否监测
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty = DependencyProperty.RegisterAttached(
            "IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, OnIsMonitoringChanged));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is System.Windows.Controls.PasswordBox passwordBox)
            {
                if (e.NewValue is bool boolValue)
                {
                    if (boolValue)
                    {
                        passwordBox.PasswordChanged += PasswordChanged;
                    }
                    else
                    {
                        passwordBox.PasswordChanged -= PasswordChanged;
                    }
                }
            }
        }

        public static void SetIsMonitoring(DependencyObject element, bool value) => element.SetValue(IsMonitoringProperty, value);

        public static bool GetIsMonitoring(DependencyObject element) => (bool)element.GetValue(IsMonitoringProperty);

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.PasswordBox passwordBox)
            {
                SetPasswordLength(passwordBox, passwordBox.Password.Length);
            }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordBoxMonitor),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordBoxMonitor), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
           typeof(PasswordBoxMonitor));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.PasswordBox passwordBox = sender as System.Windows.Controls.PasswordBox;
            passwordBox.PasswordChanged -= MyPasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += MyPasswordChanged;
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.PasswordBox passwordBox = sender as System.Windows.Controls.PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= MyPasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += MyPasswordChanged;
            }
        }

        private static void MyPasswordChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PasswordBox passwordBox = sender as System.Windows.Controls.PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}