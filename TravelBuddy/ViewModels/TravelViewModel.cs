using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using TravelBuddy.Models;

namespace TravelBuddy.ViewModels;

public class TravelViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TravelPlan> TravelPlans { get; set; }
    private string _searchText = string.Empty;
    private TravelPlan? _selectedPlan;

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value ?? string.Empty;
            OnPropertyChanged(nameof(SearchText));
            FilterPlans();
        }
    }

    public TravelPlan? SelectedPlan
    {
        get => _selectedPlan;
        set
        {
            _selectedPlan = value;
            OnPropertyChanged(nameof(SelectedPlan));
        }
    }

    public ICommand AddPlanCommand { get; }
    public ICommand ToggleFavoriteCommand { get; }

    public TravelViewModel()
    {
        TravelPlans = new ObservableCollection<TravelPlan>
        {
            new TravelPlan { Id = Guid.NewGuid().ToString(), Destination = "Paris", TravelDate = DateTime.Now.AddDays(10), TravelTime = new TimeSpan(14, 0, 0), IsFavorite = false },
            new TravelPlan { Id = Guid.NewGuid().ToString(), Destination = "Tokyo", TravelDate = DateTime.Now.AddDays(20), TravelTime = new TimeSpan(10, 0, 0), IsFavorite = true }
        };
        AddPlanCommand = new Command(async () => await AddPlan());
        ToggleFavoriteCommand = new Command<TravelPlan>(async (plan) => await ToggleFavorite(plan));
    }

    private async Task AddPlan()
    {
        if (Application.Current.Windows.Count == 0 || Application.Current.Windows[0]?.Page == null)
        {
            // Log or handle the case where no window/page is available
            return;
        }

#pragma warning disable CS8602 // Suppress null reference warning
        var page = Application.Current.Windows[0].Page;
#pragma warning restore CS8602

        var destination = await page.DisplayPromptAsync("New Plan", "Enter destination:");
        if (!string.IsNullOrEmpty(destination))
        {
            TravelPlans.Add(new TravelPlan
            {
                Id = Guid.NewGuid().ToString(),
                Destination = destination,
                TravelDate = DateTime.Now,
                TravelTime = new TimeSpan(12, 0, 0)
            });
        }
    }

    private async Task ToggleFavorite(TravelPlan plan)
    {
        if (plan == null)
            return;

        if (Application.Current.Windows.Count == 0 || Application.Current.Windows[0]?.Page == null)
        {
            // Log or handle the case where no window/page is available
            return;
        }

#pragma warning disable CS8602 // Suppress null reference warning
        var page = Application.Current.Windows[0].Page;
#pragma warning restore CS8602

        plan.IsFavorite = !plan.IsFavorite;
        await page.DisplayAlert("Favorite", $"Plan to {plan.Destination} is {(plan.IsFavorite ? "favorited" : "unfavorited")}.", "OK");
    }

    private void FilterPlans()
    {
        // Placeholder for search filtering logic
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}