namespace BusinessLogic.Models;

public class CreateEventModel
{
    public string Nome { get; set; } = null!;
    public string DataStr { get; set; } = null!;
    private DateTime DataTime => DateTime.Parse(DataStr);
    public DateOnly Data => DateOnly.FromDateTime(DataTime);
    public string Hora { get; set; } = null!;
    public string Local { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; }
    public int Capacidade { get; set; }
    public CreateIngressoModel[]? Ingressos { get; set; }
    public CreateAtividadeModel[]? Atividades { get; set; }
}