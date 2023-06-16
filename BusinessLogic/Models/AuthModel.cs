namespace BusinessLogic.Models;

public class AuthModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid Id { get; set; }
    public string Tipo { get; set; } = null!;
}