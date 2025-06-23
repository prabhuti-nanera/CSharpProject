namespace UltimateControlsDemo
{
    public partial class AlertPage : ContentPage
    {
        public AlertPage()
        {
            InitializeComponent();
        }

        private void OnShowAlert(object sender, System.EventArgs e)
        {
            DisplayAlert("Hello!", "This is an alert message.", "OK");
        }
    }
}
