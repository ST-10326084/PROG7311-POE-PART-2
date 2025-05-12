namespace Core.Models;

public class User
{
    // user model
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Farmer" or "Employee"
}
