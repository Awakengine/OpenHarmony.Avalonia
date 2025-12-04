using ReactiveUI;
using System.Collections.ObjectModel;

namespace DBManager.ViewModels;

public class DatabaseObjectViewModel : ViewModelBase
{
    public string Name { get; set; } = string.Empty;
    
    public string ObjectType { get; set; } = string.Empty;
    
    public ObservableCollection<DatabaseObjectViewModel> Children { get; }
    
    public DatabaseObjectViewModel()
    {
        Children = new ObservableCollection<DatabaseObjectViewModel>();
    }
}

public class DatabaseRootViewModel : DatabaseObjectViewModel
{
    public string ConnectionName { get; set; } = string.Empty;
}

public class DatabaseViewModel : DatabaseObjectViewModel
{
    public string DatabaseName { get; set; } = string.Empty;
}

public class TableViewModel : DatabaseObjectViewModel
{
    public string TableName { get; set; } = string.Empty;
}

public class ColumnViewModel : DatabaseObjectViewModel
{
    public string ColumnName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
}