using Microsoft.Maui;
using Microsoft.UI.Xaml;

namespace WellnessTracker;

public partial class WinUIApp : MauiWinUIApplication
{
    public WinUIApp()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}