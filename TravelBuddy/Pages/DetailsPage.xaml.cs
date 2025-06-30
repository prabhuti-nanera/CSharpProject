using Microsoft.Maui.Controls;

namespace TravelBuddy.Pages;

public partial class DetailsPage : ContentPage
{
    public DetailsPage()
    {
        InitializeComponent();
    }

    private async void OnSwipeDelete(object sender, EventArgs e)
    {
        if (Application.Current.Windows.Count == 0 || Application.Current.Windows[0]?.Page == null)
        {
            // Log or handle the case where no window/page is available
            return;
        }

#pragma warning disable CS8602 // Suppress null reference warning
        var page = Application.Current.Windows[0].Page;
#pragma warning restore CS8602

        await page.DisplayAlert("Swipe", "Delete action triggered!", "OK");
    }

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        // Handle editor text changes
    }
}