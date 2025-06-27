using Microsoft.Maui.Controls;

namespace FitnessTrackerApp;

public partial class WorkoutListPage : TabbedPage
{
    public WorkoutListPage()
    {
        InitializeComponent();
        Children[0] = new CardioPage(); // Directly set CardioPage
    }
}