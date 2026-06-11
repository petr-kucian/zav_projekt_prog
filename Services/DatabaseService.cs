using System;
using System.IO;
using DotNetEnv;
using Npgsql;

namespace Zav_projekt.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        // 1) cesta od exe (bin/Debug/...)
        var baseDir = AppContext.BaseDirectory;

        // 2) zkus najít .env v projektu (typicky 3 úrovně nahoru)
        var envPath = Path.GetFullPath(
            Path.Combine(baseDir, "..", "..", "..", ".env")
        );

        // 3) pokud existuje, načti ho
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }
        else
        {
            // fallback – zkus klasické načtení (když bys měl .env v bin)
            Env.Load();
        }

        var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(conn))
            throw new Exception(
                $"CONNECTION_STRING nebyl načten.\nHledaná cesta: {envPath}"
            );

        _connectionString = conn;
    }

    public NpgsqlConnection CreateConnection()
        => new(_connectionString);
}