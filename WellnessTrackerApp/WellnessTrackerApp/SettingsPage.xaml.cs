using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace WellnessTracker;

public partial class SettingsPage : ContentPage
{
    public ICommand ResetCommand { get; }

    public SettingsPage()
    {
        InitializeComponent();
        ResetCommand = new Command(async () =>
        {
            bool confirm = await DisplayAlert("Confirm", "Clear all data?", "Yes", "No");
            if (confirm)
                await DisplayAlert("Success", "Data cleared!", "OK");
        });
        BindingContext = this;
    }

    private async void OnNotificationsToggled(object sender, ToggledEventArgs e)
    {
        await DisplayAlert("Notifications", e.Value ? "Enabled" : "Disabled", "OK");
    }
}