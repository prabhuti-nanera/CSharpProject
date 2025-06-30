using Microsoft.Maui.Controls;
using TravelBuddy.ViewModels;
using TravelBuddy.Models; // Added missing using directive

namespace TravelBuddy.Pages;

public partial class TravelTabbedPage : TabbedPage
{
    private readonly TravelViewModel _viewModel;

    public TravelTabbedPage(TravelViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private void OnFavoriteChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is TravelPlan plan)
        {
            _viewModel.ToggleFavoriteCommand.Execute(plan);
        }
    }
}