using DBManager.Models;
using DBManager.Services;
using ReactiveUI;
using System.Data;
using System.Collections.ObjectModel;
using System;

namespace DBManager.ViewModels;

public class TableEditorViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    
    private DataTable? _tableData;
    public DataTable? TableData
    {
        get => _tableData;
        set => this.RaiseAndSetIfChanged(ref _tableData, value);
    }
    
    private ConnectionInfo? _connectionInfo;
    public ConnectionInfo? ConnectionInfo
    {
        get => _connectionInfo;
        set => this.RaiseAndSetIfChanged(ref _connectionInfo, value);
    }
    
    private string _tableName = "";
    public string TableName
    {
        get => _tableName;
        set => this.RaiseAndSetIfChanged(ref _tableName, value);
    }
    
    private string _statusText = "";
    public string StatusText
    {
        get => _statusText;
        set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }
    
    public TableEditorViewModel()
    {
        _databaseService = new DatabaseService();
    }
    
    public async void LoadTableData()
    {
        if (ConnectionInfo == null || string.IsNullOrEmpty(TableName))
            return;
            
        try
        {
            var query = $"SELECT * FROM {TableName}";
            using var reader = await _databaseService.ExecuteQueryAsync(ConnectionInfo, query);
            
            // 将IDataReader转换为DataTable
            var dataTable = new DataTable();
            dataTable.Load(reader);
            
            TableData = dataTable;
            StatusText = $"已加载 {dataTable.Rows.Count} 行数据";
        }
        catch (Exception ex)
        {
            StatusText = $"加载数据错误: {ex.Message}";
        }
    }
    
    public async void SaveChanges()
    {
        if (ConnectionInfo == null || TableData == null)
            return;
            
        try
        {
            // 在实际应用中，这里需要实现数据更新逻辑
            // 通常需要跟踪数据的变化并生成相应的UPDATE/INSERT/DELETE语句
            StatusText = "数据保存功能将在完整版本中实现";
        }
        catch (Exception ex)
        {
            StatusText = $"保存数据错误: {ex.Message}";
        }
    }
}