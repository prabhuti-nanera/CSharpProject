using Microsoft.Maui.Controls;
using ExploreApp.ViewModels;

namespace ExploreApp;

public partial class DetailsPage : TabbedPage
{
    public DetailsPage()
    {
        InitializeComponent();
        BindingContext = new DetailsViewModel();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        // Optional: Implement filtering logic for the ListView
        var viewModel = BindingContext as DetailsViewModel;
        // Add filtering based on e.NewTextValue if needed
    }
}