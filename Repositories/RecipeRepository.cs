using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Zav_projekt.Models;
using Zav_projekt.Services;

namespace Zav_projekt.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly DatabaseService _db;

    public RecipeRepository(DatabaseService db)
    {
        _db = db;
    }

    public async Task<List<Recipe>> GetAll()
    {
        var list = new List<Recipe>();

        await using var conn = _db.CreateConnection();
        await conn.OpenAsync();

        const string sql = "SELECT id, name, description, category_id FROM recipes";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Recipe
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                CategoryId = reader.GetInt32(3)
            });
        }

        return list;
    }

    public async Task Create(Recipe r)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync();

        const string sql =
            "INSERT INTO recipes(name, description, category_id) VALUES (@n,@d,@c)";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@n", r.Name);
        cmd.Parameters.AddWithValue("@d", r.Description);
        cmd.Parameters.AddWithValue("@c", r.CategoryId);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task Update(Recipe r)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync();

        const string sql =
            "UPDATE recipes SET name=@n, description=@d, category_id=@c WHERE id=@id";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@id", r.Id);
        cmd.Parameters.AddWithValue("@n", r.Name);
        cmd.Parameters.AddWithValue("@d", r.Description);
        cmd.Parameters.AddWithValue("@c", r.CategoryId);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task Delete(int id)
    {
        await using var conn = _db.CreateConnection();
        await conn.OpenAsync();

        const string sql = "DELETE FROM recipes WHERE id=@id";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        await cmd.ExecuteNonQueryAsync();
    }
}