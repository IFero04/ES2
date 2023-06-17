namespace BusinessLogic.Models;

public class IngressoDetalheModel
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public double Preco { get; set; }

    public int Quantidade { get; set; }
}