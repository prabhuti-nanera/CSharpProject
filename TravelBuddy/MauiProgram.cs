using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using TravelBuddy.ViewModels;
using TravelBuddy.Pages;

namespace TravelBuddy;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register ViewModels
        builder.Services.AddSingleton<TravelViewModel>();

        // Register Pages
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<TravelFlyoutPage>();
        builder.Services.AddSingleton<TravelTabbedPage>();
        builder.Services.AddSingleton<CalculatorPage>();
        builder.Services.AddSingleton<DetailsPage>();

        return builder.Build();
    }
}