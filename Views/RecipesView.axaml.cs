using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Zav_projekt.ViewModels;

namespace Zav_projekt.Views;

public partial class RecipesView : UserControl
{
    public RecipesView()
    {
        InitializeComponent();

        DataContext = App.Services.GetRequiredService<RecipesViewModel>();
    }
}