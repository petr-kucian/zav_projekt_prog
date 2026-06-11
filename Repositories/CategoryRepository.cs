using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Zav_projekt.Models;
using Zav_projekt.Services;

namespace Zav_projekt.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DatabaseService _db;

    public CategoryRepository(DatabaseService db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAll()
    {
        var list = new List<Category>();

        await using var conn = _db.CreateConnection();
        await conn.OpenAsync();

        const string sql = "SELECT id, name FROM categories ORDER BY id";

        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return list;
    }
}