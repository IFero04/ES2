namespace BusinessLogic.Models;

public class CreateUtilizadorModel
{
    public string Nome { get; set;}
    public string Email { get; set;}
    public string Username { get; set; }
    public string Senha { get; set;}
    public string Telefone { get; set;}
    public bool Organizador { get; set; }
}