using System.Collections.Generic;
using System.Threading.Tasks;
using Zav_projekt.Models;

namespace Zav_projekt.Repositories;

public interface IIngredientRepository
{
    Task<List<Ingredient>> GetByRecipeId(int recipeId);

    Task Create(Ingredient ingredient);

    Task Delete(int id);
}