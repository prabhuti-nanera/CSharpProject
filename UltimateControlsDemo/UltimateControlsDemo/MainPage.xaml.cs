using Microsoft.Maui.Controls;

namespace UltimateControlsDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.FadeTo(1, 500);
        }

        private async void OnNavigateWithName(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NameEntry?.Text))
            {
                await Shell.Current.GoToAsync($"//AlertPage?name={Uri.EscapeDataString(NameEntry.Text)}");
            }
            else
            {
                await DisplayAlert("Error", "Please enter a name.", "OK");
            }
        }

        private void OnToggleTheme(object sender, EventArgs e)
        {
            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
        }
    }
}