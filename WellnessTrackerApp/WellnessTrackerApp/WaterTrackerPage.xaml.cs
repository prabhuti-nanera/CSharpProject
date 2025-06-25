using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace WellnessTracker;

public partial class WaterTrackerPage : ContentPage
{
    public ICommand SaveWaterCommand { get; }

    public WaterTrackerPage()
    {
        InitializeComponent();
        SaveWaterCommand = new Command(async () =>
        {
            int glasses = int.TryParse(WaterEntry?.Text, out int entry) ? entry : (int)(WaterStepper?.Value ?? 0);
            await DisplayAlert("Success", $"Logged {glasses} glasses of water!", "OK");
        });
        BindingContext = this;
    }
}