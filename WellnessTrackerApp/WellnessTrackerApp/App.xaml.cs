using Microsoft.Maui.Controls;

namespace WellnessTracker;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var flyoutPage = new FlyoutPage
        {
            Flyout = new ContentPage
            {
                Title = "Menu",
                Content = new StackLayout
                {
                    Children =
                    {
                        new Button { Text = "Home", Command = new Command(async () => await (flyoutPage.Detail as NavigationPage)!.Navigation.PushAsync(new MainPage())) },
                        new Button { Text = "Settings", Command = new Command(async () => await (flyoutPage.Detail as NavigationPage)!.Navigation.PushAsync(new SettingsPage())) }
                    }
                }
            },
            Detail = new NavigationPage(new MainPage())
        };
        return new Window { Page = flyoutPage };
    }
}