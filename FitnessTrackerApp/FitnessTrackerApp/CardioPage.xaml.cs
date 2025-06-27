using Microsoft.Maui.Controls;

namespace FitnessTrackerApp;

public partial class CardioPage : ContentPage
{
    public CardioPage()
    {
        InitializeComponent();
    }

    private void OnSliderValueChanged(object? sender, ValueChangedEventArgs e)
    {
        durationLabel.Text = $"Duration: {e.NewValue:F0} minutes";
    }

    private async void OnLogWorkoutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Workout Logged", "New workout has been added.", "OK");
    }

    private void OnSearchPressed(object sender, EventArgs e)
    {
        // Implement search logic
    }

    private async void OnSwipeDelete(object sender, EventArgs e)
    {
        await DisplayAlert("Workout Deleted", "Workout has been removed.", "OK");
    }
}