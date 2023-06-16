namespace BusinessLogic.Models;

public class ViewEventoModel
{
    public string Nome { get; set; } = null!;

    public DateOnly Data { get; set; }

    public string Hora { get; set; } = null!;

    public string Local { get; set; } = null!;

    public string? Descricao { get; set; }

    public string? Categoria { get; set; } = null!;

    public int Capacidade { get; set; }
    
    public int NumeroAtidades { get; set; }
    
    public double IngressoBarato { get; set; }
}