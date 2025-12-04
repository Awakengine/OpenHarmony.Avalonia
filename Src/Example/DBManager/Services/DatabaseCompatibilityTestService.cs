using DBManager.Models;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DBManager.Services;

public class DatabaseCompatibilityTestService
{
    private readonly DatabaseService _databaseService;
    
    public DatabaseCompatibilityTestService()
    {
        _databaseService = new DatabaseService();
    }
    
    public async Task<Dictionary<DatabaseType, bool>> TestAllDatabaseTypesAsync()
    {
        var results = new Dictionary<DatabaseType, bool>();
        
        // 测试SQL Server
        var sqlServerConnection = new ConnectionInfo
        {
            DatabaseType = DatabaseType.SQLServer,
            Server = "localhost",
            Port = 1433,
            Database = "master",
            Username = "sa",
            Password = "StrongPassword123!"
        };
        results[DatabaseType.SQLServer] = await _databaseService.TestConnectionAsync(sqlServerConnection);
        
        // 测试MySQL
        var mySqlConnection = new ConnectionInfo
        {
            DatabaseType = DatabaseType.MySQL,
            Server = "localhost",
            Port = 3306,
            Database = "mysql",
            Username = "root",
            Password = "password"
        };
        results[DatabaseType.MySQL] = await _databaseService.TestConnectionAsync(mySqlConnection);
        
        // 测试PostgreSQL
        var postgreSqlConnection = new ConnectionInfo
        {
            DatabaseType = DatabaseType.PostgreSQL,
            Server = "localhost",
            Port = 5432,
            Database = "postgres",
            Username = "postgres",
            Password = "password"
        };
        results[DatabaseType.PostgreSQL] = await _databaseService.TestConnectionAsync(postgreSqlConnection);
        
        // 测试Oracle
        var oracleConnection = new ConnectionInfo
        {
            DatabaseType = DatabaseType.Oracle,
            Server = "localhost",
            Port = 1521,
            Database = "XE",
            Username = "system",
            Password = "oracle"
        };
        results[DatabaseType.Oracle] = await _databaseService.TestConnectionAsync(oracleConnection);
        
        return results;
    }
    
    public async Task<TestResult> TestDatabaseFunctionalityAsync(ConnectionInfo connectionInfo)
    {
        var result = new TestResult
        {
            DatabaseType = connectionInfo.DatabaseType,
            ConnectionSuccess = false,
            CanExecuteQueries = false,
            CanRetrieveSchema = false,
            ErrorMessage = ""
        };
        
        try
        {
            // 测试连接
            result.ConnectionSuccess = await _databaseService.TestConnectionAsync(connectionInfo);
            if (!result.ConnectionSuccess)
            {
                result.ErrorMessage = "无法连接到数据库";
                return result;
            }
            
            // 测试执行简单查询
            try
            {
                var query = connectionInfo.DatabaseType switch
                {
                    DatabaseType.SQLServer => "SELECT 1 AS Result",
                    DatabaseType.MySQL => "SELECT 1 AS Result",
                    DatabaseType.PostgreSQL => "SELECT 1 AS Result",
                    DatabaseType.Oracle => "SELECT 1 AS Result FROM DUAL",
                    _ => "SELECT 1 AS Result"
                };
                
                using var reader = await _databaseService.ExecuteQueryAsync(connectionInfo, query);
                result.CanExecuteQueries = true; // 简化处理，假设如果能执行到这里就成功
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"查询执行失败: {ex.Message}";
                return result;
            }
            
            // 测试获取数据库列表
            try
            {
                var databases = await _databaseService.GetDatabasesAsync(connectionInfo);
                result.CanRetrieveSchema = databases.Count > 0;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"获取数据库列表失败: {ex.Message}";
                return result;
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = $"测试过程中发生错误: {ex.Message}";
        }
        
        return result;
    }
}

public class TestResult
{
    public DatabaseType DatabaseType { get; set; }
    public bool ConnectionSuccess { get; set; }
    public bool CanExecuteQueries { get; set; }
    public bool CanRetrieveSchema { get; set; }
    public string ErrorMessage { get; set; } = "";
    
    public bool OverallSuccess => ConnectionSuccess && CanExecuteQueries && CanRetrieveSchema;
}