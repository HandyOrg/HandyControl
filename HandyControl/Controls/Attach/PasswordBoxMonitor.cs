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
    }
}