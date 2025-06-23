using Microsoft.Maui.Controls;

namespace UltimateControlsDemo
{
    public partial class ControlsPage : ContentPage
    {
        public ControlsPage()
        {
            InitializeComponent();

            // Bind switch event
            ToggleSwitch.Toggled += (s, e) =>
            {
                SwitchLabel.Text = $"Switch is {(e.Value ? "On" : "Off")}";
            };
        }

        private void OnShowNameClicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Name", $"Your name is {NameEntry.Text}", "OK");
        }

        private void OnDeleteSwipe(SwipeItem swipeItem)
        {
            DisplayAlert("Deleted", "Item has been deleted", "OK");
        }
    }
}
