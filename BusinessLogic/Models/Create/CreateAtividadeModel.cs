namespace BusinessLogic.Models;

public class CreateAtividadeModel
{
    public string Nome { get; set; } = null!;
    public string DataStr { get; set; } = null!;
    private DateTime DataTime => DateTime.Parse(DataStr);
    public DateOnly Data => DateOnly.FromDateTime(DataTime);
    public string Hora { get; set; } = null!;
    public string? Descricao { get; set; }
}