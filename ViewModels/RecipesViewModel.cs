using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Zav_projekt.Models;
using Zav_projekt.Repositories;

namespace Zav_projekt.ViewModels;

public partial class RecipesViewModel : ObservableObject
{
    private readonly IRecipeRepository _repo;

    [ObservableProperty]
    private ObservableCollection<Recipe> recipes = new();

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private int categoryId = 1;

    [ObservableProperty]
    private Recipe? selectedRecipe;

    public RecipesViewModel(IRecipeRepository repo)
    {
        _repo = repo;
        _ = Load();
    }

    private async Task Load()
    {
        var data = await _repo.GetAll();
        Recipes = new ObservableCollection<Recipe>(data);
    }

    [RelayCommand]
    private async Task Add()
    {
        await _repo.Create(new Recipe
        {
            Name = Name,
            Description = Description,
            CategoryId = CategoryId
        });

        await Load();
    }

    [RelayCommand]
    private async Task Update()
    {
        if (SelectedRecipe is null) return;

        SelectedRecipe.Name = Name;
        SelectedRecipe.Description = Description;
        SelectedRecipe.CategoryId = CategoryId;

        await _repo.Update(SelectedRecipe);
        await Load();
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedRecipe is null) return;

        await _repo.Delete(SelectedRecipe.Id);
        await Load();
    }

    partial void OnSelectedRecipeChanged(Recipe? value)
    {
        if (value is null) return;

        Name = value.Name;
        Description = value.Description;
        CategoryId = value.CategoryId;
    }
}