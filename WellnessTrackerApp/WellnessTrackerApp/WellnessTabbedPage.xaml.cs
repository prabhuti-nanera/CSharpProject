using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace WellnessTracker;

public partial class WellnessTabbedPage : TabbedPage
{
    public ICommand SaveExerciseCommand { get; }
    public ICommand SaveSleepCommand { get; }

    public WellnessTabbedPage()
    {
        InitializeComponent();
        SaveExerciseCommand = new Command(async () => await DisplayAlert("Exercise Saved", $"Exercise logged on {ExerciseDate?.Date.ToString("d") ?? "N/A"} at {ExerciseTime?.Time.ToString() ?? "N/A"}", "OK"));
        SaveSleepCommand = new Command(async () => await DisplayAlert("Sleep Saved", $"Slept {SleepStepper?.Value ?? 0} hours", "OK"));
        BindingContext = this;
    }
}