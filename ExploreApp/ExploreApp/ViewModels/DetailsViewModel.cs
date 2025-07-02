using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ExploreApp.Models;

namespace ExploreApp.ViewModels;

public class DetailsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Destination> Destinations { get; set; } = new ObservableCollection<Destination>();
    private string _result = string.Empty;
    public string Result
    {
        get => _result;
        set
        {
            _result = value ?? string.Empty;
            OnPropertyChanged(nameof(Result));
        }
    }

    public ICommand CalculateCommand { get; }
    public ICommand NumberCommand { get; }
    public ICommand OperatorCommand { get; }

    public DetailsViewModel()
    {
        Destinations.Add(new Destination { Name = "Paris", Description = "City of Light" });
        Destinations.Add(new Destination { Name = "Tokyo", Description = "Vibrant Metropolis" });
        CalculateCommand = new Command(OnCalculate);
        NumberCommand = new Command<string>(OnNumber);
        OperatorCommand = new Command<string>(OnOperator);
    }

    private void OnNumber(string? number)
    {
        if (number != null)
        {
            Result += number;
        }
    }

    private void OnOperator(string? op)
    {
        if (op != null)
        {
            Result += $" {op} ";
        }
    }

    private async void OnCalculate()
    {
        if (string.IsNullOrEmpty(Result))
        {
            Result = "Error";
            await DisplayErrorAlert("Invalid input");
            return;
        }

        try
        {
            var computeResult = new DataTable().Compute(Result, null);
            Result = computeResult?.ToString() ?? "Error";
            await DisplayResultAlert(Result);
        }
        catch
        {
            Result = "Error";
            await DisplayErrorAlert("Invalid calculation");
        }
    }

    private async Task DisplayResultAlert(string result)
    {
        var currentApp = Application.Current;
        if (currentApp?.Windows?.Count > 0)
        {
            var window = currentApp.Windows[0];
            if (window?.Page != null)
            {
                await window.Page.DisplayAlert("Result", result, "OK");
            }
        }
    }

    private async Task DisplayErrorAlert(string message)
    {
        var currentApp = Application.Current;
        if (currentApp?.Windows?.Count > 0)
        {
            var window = currentApp.Windows[0];
            if (window?.Page != null)
            {
                await window.Page.DisplayAlert("Error", message, "OK");
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}