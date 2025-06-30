using Microsoft.Maui.Controls;
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class HomePage : ContentPage
{
    private readonly TravelViewModel _viewModel;

    public HomePage(TravelViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private async void OnGoToFlyoutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TravelFlyoutPage());
    }

    private async void OnGoToTabsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TravelTabbedPage(_viewModel));
    }

    private async void OnGoToCalculatorClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CalculatorPage());
    }
}