using DBManager.Models;
using DBManager.Services;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace DBManager.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    
    public ObservableCollection<ConnectionInfo> Connections { get; }
    
    private ConnectionInfo? _selectedConnection;
    public ConnectionInfo? SelectedConnection
    {
        get => _selectedConnection;
        set => this.RaiseAndSetIfChanged(ref _selectedConnection, value);
    }
    
    private string _statusText = "就绪";
    public string StatusText
    {
        get => _statusText;
        set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }
    
    public MainViewModel()
    {
        _databaseService = new DatabaseService();
        Connections = new ObservableCollection<ConnectionInfo>();
        
        // 添加示例连接
        Connections.Add(new ConnectionInfo
        {
            Id = 1,
            Name = "本地MySQL",
            DatabaseType = DatabaseType.MySQL,
            Server = "localhost",
            Port = 3306,
            Database = "test",
            Username = "root",
            Password = "password"
        });
        
        Connections.Add(new ConnectionInfo
        {
            Id = 2,
            Name = "本地PostgreSQL",
            DatabaseType = DatabaseType.PostgreSQL,
            Server = "localhost",
            Port = 5432,
            Database = "test",
            Username = "postgres",
            Password = "password"
        });
    }
}