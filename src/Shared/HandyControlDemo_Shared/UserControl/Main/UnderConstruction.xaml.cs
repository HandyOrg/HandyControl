using HandyControl.Controls;

namespace HandyControlDemo.UserControl
{
    public partial class UnderConstruction
    {
        public UnderConstruction()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ThemeManager themeManager = ThemeManager.Current;
            themeManager.ApplicationTheme = ApplicationTheme.Dark;
        }

        private void Button_Click1(object sender, System.Windows.RoutedEventArgs e)
        {
            ThemeManager themeManager = ThemeManager.Current;
            themeManager.ApplicationTheme = ApplicationTheme.Default;
        }

        private void Button_Click2(object sender, System.Windows.RoutedEventArgs e)
        {
            ThemeManager themeManager = ThemeManager.Current;
            themeManager.ApplicationTheme = ApplicationTheme.Violet;
        }
    }
}
