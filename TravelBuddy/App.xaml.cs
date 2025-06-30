using Microsoft.Maui.Controls;
using TravelBuddy.Pages;

namespace TravelBuddy;

public partial class App : Application
{
    private readonly HomePage _homePage;

    public App(HomePage homePage)
    {
        InitializeComponent();
        _homePage = homePage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(_homePage) { BarBackgroundColor = Colors.Navy });
    }
}