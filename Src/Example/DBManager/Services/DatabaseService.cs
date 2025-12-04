using DBManager.Models;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Data.Common;

namespace DBManager.Services;

public class DatabaseService
{
    public async Task<bool> TestConnectionAsync(ConnectionInfo connectionInfo)
    {
        try
        {
            using var connection = DatabaseAdapterFactory.CreateConnection(connectionInfo);
            await Task.Run(() => connection.Open());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IDataReader> ExecuteQueryAsync(ConnectionInfo connectionInfo, string query)
    {
        var connection = DatabaseAdapterFactory.CreateConnection(connectionInfo);
        await Task.Run(() => connection.Open());
        var command = connection.CreateCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();
        return reader;
    }

    public async Task<int> ExecuteNonQueryAsync(ConnectionInfo connectionInfo, string query)
    {
        using var connection = DatabaseAdapterFactory.CreateConnection(connectionInfo);
        await Task.Run(() => connection.Open());
        var command = connection.CreateCommand();
        command.CommandText = query;
        var result = command.ExecuteNonQuery();
        return await Task.FromResult(result);
    }

    public async Task<List<string>> GetDatabasesAsync(ConnectionInfo connectionInfo)
    {
        var databases = new List<string>();
        var query = connectionInfo.DatabaseType switch
        {
            DatabaseType.SQLServer => "SELECT name FROM sys.databases",
            DatabaseType.MySQL => "SHOW DATABASES",
            DatabaseType.PostgreSQL => "SELECT datname FROM pg_database WHERE datistemplate = false",
            DatabaseType.Oracle => "SELECT USERNAME FROM ALL_USERS ORDER BY USERNAME",
            _ => throw new NotSupportedException($"Database type {connectionInfo.DatabaseType} is not supported.")
        };

        using var reader = await ExecuteQueryAsync(connectionInfo, query);
        while (reader.Read())
        {
            databases.Add(reader.GetString(0));
        }

        return databases;
    }

    public async Task<List<string>> GetTablesAsync(ConnectionInfo connectionInfo, string databaseName)
    {
        var tables = new List<string>();
        var query = connectionInfo.DatabaseType switch
        {
            DatabaseType.SQLServer => $"SELECT TABLE_NAME FROM {databaseName}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'",
            DatabaseType.MySQL => $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{databaseName}' AND TABLE_TYPE = 'BASE TABLE'",
            DatabaseType.PostgreSQL => $"SELECT tablename FROM pg_tables WHERE schemaname = 'public'",
            DatabaseType.Oracle => "SELECT TABLE_NAME FROM USER_TABLES",
            _ => throw new NotSupportedException($"Database type {connectionInfo.DatabaseType} is not supported.")
        };

        using var reader = await ExecuteQueryAsync(connectionInfo, query);
        while (reader.Read())
        {
            tables.Add(reader.GetString(0));
        }

        return tables;
    }
}