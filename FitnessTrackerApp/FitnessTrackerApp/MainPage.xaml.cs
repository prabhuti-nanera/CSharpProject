using Microsoft.Maui.Controls;

namespace FitnessTrackerApp;

public partial class MainPage : FlyoutPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnWorkoutsClicked(object sender, EventArgs e)
    {
        Detail = new NavigationPage(new WorkoutListPage());
        // Delay to ensure navigation is complete before changing IsPresented
        await Task.Delay(100);
        IsPresented = true;
    }

    private async void OnCalculatorClicked(object sender, EventArgs e)
    {
        Detail = new NavigationPage(new CalorieCalculatorPage());
        // Delay to ensure navigation is complete before changing IsPresented
        await Task.Delay(100);
        IsPresented = true;
    }
}