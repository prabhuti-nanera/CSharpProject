using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace WellnessTracker;

public partial class MoodTrackerPage : ContentPage
{
    public ICommand SaveMoodCommand { get; }

    public MoodTrackerPage()
    {
        InitializeComponent();
        SaveMoodCommand = new Command(async () =>
        {
            string mood = HappyRadio?.IsChecked == true ? "Happy" : NeutralRadio?.IsChecked == true ? "Neutral" : SadRadio?.IsChecked == true ? "Sad" : "Unknown";
            await DisplayAlert("Mood Saved", $"Mood: {mood}, Intensity: {MoodSlider?.Value ?? 5}, Exercised: {ActivityCheck?.IsChecked ?? false}", "OK");
        });
        BindingContext = this;
    }
}