using Microsoft.Maui.Controls;

namespace TravelBuddy.Pages;

public partial class TravelFlyoutPage : FlyoutPage
{
    public TravelFlyoutPage()
    {
        InitializeComponent();
    }

    private async void OnMenuItemClicked(object sender, EventArgs e)
    {
        if (Application.Current.Windows.Count == 0 || Application.Current.Windows[0]?.Page == null)
        {
            // Log or handle the case where no window/page is available
            return;
        }

#pragma warning disable CS8602 // Suppress null reference warning
        var page = Application.Current.Windows[0].Page;
#pragma warning restore CS8602

        await page.DisplayAlert("Menu", "Menu item clicked!", "OK");
    }
}