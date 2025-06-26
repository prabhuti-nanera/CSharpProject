using Microsoft.Maui.Controls;

namespace RecipeBookApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var navPage = new NavigationPage(new MainPage());
        var flyoutPage = new FlyoutPage
        {
            Flyout = new ContentPage
            {
                Title = "Menu",
                Content = new StackLayout
                {
                    Padding = 20,
                    Children =
                    {
                        new Label { Text = "Recipe Book", FontSize = 24, HorizontalOptions = LayoutOptions.Center, TextColor = Color.FromArgb("#FF6347") },
                        new Button { Text = "Recipes", Command = new Command(async () => await navPage.Navigation.PushAsync(new MainPage())) },
                        new Button { Text = "Favorites", Command = new Command(async () => await navPage.Navigation.PushAsync(new MainPage())) }
                    }
                }
            },
            Detail = navPage
        };
        return new Window(flyoutPage);
    }
}