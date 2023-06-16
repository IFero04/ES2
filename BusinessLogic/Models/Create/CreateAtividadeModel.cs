namespace BusinessLogic.Models;

public class CreateAtividadeModel
{
    public string Nome { get; set; } = null!;
    public DateOnly Data { get; set; }
    public string Hora { get; set; } = null!;
    public string? Descricao { get; set; }
}