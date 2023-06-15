namespace BusinessLogic.Models;

public class CreateUtilizadorModel
{
    public string Nome { get; set; } = null!;
    public string Email { get; set;} = null!;
    public string Username { get; set; } = null!;
    public string Senha { get; set;} = null!;
    public string Telefone { get; set;}
    public bool Organizador { get; set; }
}