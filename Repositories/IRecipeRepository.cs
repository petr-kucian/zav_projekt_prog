using System.Collections.Generic;
using System.Threading.Tasks;
using Zav_projekt.Models;

namespace Zav_projekt.Repositories;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAll();

    Task Create(Recipe recipe);

    Task Update(Recipe recipe);

    Task Delete(int id);
}