using Microsoft.Maui.Controls;

namespace UltimateControlsDemo
{
    public partial class AlertPage : ContentPage
    {
        public AlertPage()
        {
            InitializeComponent();
            this.FadeTo(1, 500);
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            // Access query parameters via Shell navigation context
            if (Shell.Current?.CurrentState?.Location?.Query != null)
            {
                var queryParams = System.Web.HttpUtility.ParseQueryString(Shell.Current.CurrentState.Location.Query);
                var name = queryParams["name"];
                if (!string.IsNullOrEmpty(name))
                {
                    MessageLabel.Text = $"Hello, {Uri.UnescapeDataString(name)}!";
                }
            }
        }

        private async void OnShowAlert(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", MessageLabel.Text, "OK");
        }
    }
}