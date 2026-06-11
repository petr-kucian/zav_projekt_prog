using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Zav_projekt.Models;
using Zav_projekt.Repositories;

namespace Zav_projekt.ViewModels;

public partial class RecipeDetailViewModel : ObservableObject
{
    private readonly IIngredientRepository _ingredientRepository;

    public Recipe SelectedRecipe { get; }

    [ObservableProperty]
    private ObservableCollection<Ingredient> ingredients = new();

    [ObservableProperty]
    private string ingredientName = "";

    [ObservableProperty]
    private string ingredientAmount = "";

    [ObservableProperty]
    private Ingredient? selectedIngredient;

    public RecipeDetailViewModel(
        Recipe recipe,
        IIngredientRepository ingredientRepository)
    {
        SelectedRecipe = recipe;
        _ingredientRepository = ingredientRepository;

        _ = Load();
    }

    private async Task Load()
    {
        Ingredients = new ObservableCollection<Ingredient>(
            await _ingredientRepository.GetByRecipeId(SelectedRecipe.Id));
    }

    [RelayCommand]
    private async Task AddIngredient()
    {
        if (string.IsNullOrWhiteSpace(IngredientName))
            return;

        await _ingredientRepository.Create(new Ingredient
        {
            RecipeId = SelectedRecipe.Id,
            Name = IngredientName,
            Amount = IngredientAmount
        });

        IngredientName = "";
        IngredientAmount = "";

        await Load();
    }

    [RelayCommand]
    private async Task DeleteIngredient()
    {
        if (SelectedIngredient is null)
            return;

        await _ingredientRepository.Delete(
            SelectedIngredient.Id);

        await Load();
    }
}