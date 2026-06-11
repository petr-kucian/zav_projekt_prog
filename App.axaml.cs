using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Zav_projekt.Repositories;
using Zav_projekt.Services;
using Zav_projekt.ViewModels;
using Zav_projekt.Views;

namespace Zav_projekt;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        // DB
        services.AddSingleton<DatabaseService>();

        // Repositories
        services.AddSingleton<IRecipeRepository, RecipeRepository>();
        services.AddSingleton<IIngredientRepository, IngredientRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();

        // ViewModels
        services.AddSingleton<RecipesViewModel>();

        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<RecipesViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}