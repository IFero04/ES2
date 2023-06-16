namespace BusinessLogic.Models;

public class PesquisarEventoModel
{
    public string Nome { get; set; }
    public string DataStr { get; set; } = null!;
    private DateTime DataTime => DateTime.Parse(DataStr);
    public DateOnly Data => DateOnly.FromDateTime(DataTime);
    public string Local { get; set; }
    public string Categoria { get; set; }
}