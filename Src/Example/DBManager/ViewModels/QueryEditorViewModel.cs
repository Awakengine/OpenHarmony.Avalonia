using DBManager.Models;
using DBManager.Services;
using ReactiveUI;
using System.Data;
using System.Collections.ObjectModel;
using System;

namespace DBManager.ViewModels;

public class QueryEditorViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    
    private string _sqlText = "// 在此处输入SQL查询\n// 按F5执行查询";
    public string SqlText
    {
        get => _sqlText;
        set => this.RaiseAndSetIfChanged(ref _sqlText, value);
    }
    
    private DataTable? _results;
    public DataTable? Results
    {
        get => _results;
        set => this.RaiseAndSetIfChanged(ref _results, value);
    }
    
    private ConnectionInfo? _connectionInfo;
    public ConnectionInfo? ConnectionInfo
    {
        get => _connectionInfo;
        set => this.RaiseAndSetIfChanged(ref _connectionInfo, value);
    }
    
    private string _executionTime = "";
    public string ExecutionTime
    {
        get => _executionTime;
        set => this.RaiseAndSetIfChanged(ref _executionTime, value);
    }
    
    public QueryEditorViewModel()
    {
        _databaseService = new DatabaseService();
    }
    
    public async void ExecuteQuery()
    {
        if (ConnectionInfo == null || string.IsNullOrEmpty(SqlText))
            return;
            
        try
        {
            var startTime = DateTime.Now;
            using var reader = await _databaseService.ExecuteQueryAsync(ConnectionInfo, SqlText);
            
            // 将IDataReader转换为DataTable
            var dataTable = new DataTable();
            dataTable.Load(reader);
            
            Results = dataTable;
            var endTime = DateTime.Now;
            ExecutionTime = $"执行时间: {(endTime - startTime).TotalMilliseconds} ms";
        }
        catch (Exception ex)
        {
            // 在实际应用中，这里应该显示错误信息给用户
            ExecutionTime = $"执行错误: {ex.Message}";
        }
    }
}