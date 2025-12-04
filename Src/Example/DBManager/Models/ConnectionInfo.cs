using System.ComponentModel.DataAnnotations;

namespace DBManager.Models;

public class ConnectionInfo
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public DatabaseType DatabaseType { get; set; }
    
    [Required]
    public string Server { get; set; } = string.Empty;
    
    public int Port { get; set; }
    
    [Required]
    public string Database { get; set; } = string.Empty;
    
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public bool IsConnected { get; set; }
}