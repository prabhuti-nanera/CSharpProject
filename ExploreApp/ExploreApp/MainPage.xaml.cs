using Microsoft.Maui.Controls;

namespace ExploreApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnNavigateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailsPage());
    }
}