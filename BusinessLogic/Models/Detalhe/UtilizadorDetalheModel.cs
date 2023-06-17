namespace BusinessLogic.Models;

public class UtilizadorDetalheModel
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;
    
    public string? Telefone { get; set; }

    public string Tipo { get; set; } = null!;
}