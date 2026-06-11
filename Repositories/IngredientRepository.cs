using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Zav_projekt.Models;
using Zav_projekt.Services;

namespace Zav_projekt.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly DatabaseService _db;

    public IngredientRepository(DatabaseService db)
    {
        _db = db;
    }

    public async Task<List<Ingredient>> GetByRecipeId(int recipeId)
    {
        var ingredients = new List<Ingredient>();

        await using var connection = _db.CreateConnection();
        await connection.OpenAsync();

        const string sql =
            """
            SELECT id, recipe_id, name, amount
            FROM ingredients
            WHERE recipe_id = @recipeId
            ORDER BY id
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@recipeId", recipeId);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            ingredients.Add(new Ingredient
            {
                Id = reader.GetInt32(0),
                RecipeId = reader.GetInt32(1),
                Name = reader.GetString(2),
                Amount = reader.IsDBNull(3) ? "" : reader.GetString(3)
            });
        }

        return ingredients;
    }

    public async Task Create(Ingredient ingredient)
    {
        await using var connection = _db.CreateConnection();
        await connection.OpenAsync();

        const string sql =
            """
            INSERT INTO ingredients(recipe_id, name, amount)
            VALUES(@recipeId, @name, @amount)
            """;

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("@recipeId", ingredient.RecipeId);
        command.Parameters.AddWithValue("@name", ingredient.Name);
        command.Parameters.AddWithValue("@amount", ingredient.Amount);

        await command.ExecuteNonQueryAsync();
    }

    public async Task Delete(int id)
    {
        await using var connection = _db.CreateConnection();
        await connection.OpenAsync();

        const string sql =
            """
            DELETE FROM ingredients
            WHERE id = @id
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }
}