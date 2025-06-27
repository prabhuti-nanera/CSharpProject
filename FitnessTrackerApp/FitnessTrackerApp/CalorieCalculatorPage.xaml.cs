using Microsoft.Maui.Controls;

namespace FitnessTrackerApp;

public partial class CalorieCalculatorPage : ContentPage
{
    public CalorieCalculatorPage()
    {
        InitializeComponent();
    }

    private async void OnCalculateClicked(object sender, EventArgs e)
    {
        if (double.TryParse(weightEntry.Text, out double weight) && double.TryParse(durationEntry.Text, out double duration))
        {
            var button = sender as Button;
            if (button == null)
            {
                await DisplayAlert("Error", "Invalid button.", "OK");
                return;
            }
            double met = button.Text switch
            {
                "Run" => 8.0,
                "Cycle" => 6.0,
                "Swim" => 7.0,
                "Walk" => 3.5,
                _ => 1.0
            };
            double caloricBurn = (met * weight * duration) / 60;
            calorieLabel.Text = $"Calories: {caloricBurn:F2}";
        }
        else
        {
            await DisplayAlert("Error", "Please enter valid weight and duration.", "OK");
        }
    }
}