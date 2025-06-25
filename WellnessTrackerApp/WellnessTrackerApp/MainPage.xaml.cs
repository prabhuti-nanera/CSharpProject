using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace WellnessTracker;

public partial class MainPage : ContentPage
{
    public ICommand NavigateToWaterCommand { get; }
    public ICommand NavigateToMoodCommand { get; }
    public ICommand NavigateToTabbedCommand { get; }

    public MainPage()
    {
        InitializeComponent();
        NavigateToWaterCommand = new Command(async () => await Navigation.PushAsync(new WaterTrackerPage()));
        NavigateToMoodCommand = new Command(async () => await Navigation.PushAsync(new MoodTrackerPage()));
        NavigateToTabbedCommand = new Command(async () => await Navigation.PushAsync(new WellnessTabbedPage()));
        BindingContext = this;
    }
}