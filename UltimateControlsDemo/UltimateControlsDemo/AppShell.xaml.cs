namespace UltimateControlsDemo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Subscribe to theme changes via Application.Current
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
            UpdateTheme();
        }

        private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            UpdateTheme();
        }

        private void UpdateTheme()
        {
            if (Application.Current?.RequestedTheme != null)
            {
                Application.Current.UserAppTheme = Application.Current.RequestedTheme == AppTheme.Unspecified
                    ? AppTheme.Light
                    : Application.Current.RequestedTheme;
            }
        }
    }
}