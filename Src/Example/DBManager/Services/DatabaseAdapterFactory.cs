using DBManager.Models;
using System.Data;
using System;

namespace DBManager.Services;

public class DatabaseAdapterFactory
{
    public static IDbConnection CreateConnection(ConnectionInfo connectionInfo)
    {
        return connectionInfo.DatabaseType switch
        {
            DatabaseType.SQLServer => new System.Data.SqlClient.SqlConnection(
                $"Server={connectionInfo.Server},{connectionInfo.Port};Database={connectionInfo.Database};User Id={connectionInfo.Username};Password={connectionInfo.Password};TrustServerCertificate=true;"),
            DatabaseType.MySQL => new MySql.Data.MySqlClient.MySqlConnection(
                $"Server={connectionInfo.Server};Port={connectionInfo.Port};Database={connectionInfo.Database};Uid={connectionInfo.Username};Pwd={connectionInfo.Password};"),
            DatabaseType.PostgreSQL => new Npgsql.NpgsqlConnection(
                $"Host={connectionInfo.Server};Port={connectionInfo.Port};Database={connectionInfo.Database};Username={connectionInfo.Username};Password={connectionInfo.Password};"),
            DatabaseType.Oracle => new Oracle.ManagedDataAccess.Client.OracleConnection(
                $"Data Source={connectionInfo.Server}:{connectionInfo.Port}/{connectionInfo.Database};User Id={connectionInfo.Username};Password={connectionInfo.Password};"),
            _ => throw new NotSupportedException($"Database type {connectionInfo.DatabaseType} is not supported.")
        };
    }
}