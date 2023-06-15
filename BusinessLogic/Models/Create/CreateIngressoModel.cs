namespace BusinessLogic.Models;

public class CreateIngressoModel
{
    public string Nome { get; set; } = null!;
    public double Preco { get; set; }
    public int Quantidade { get; set; }
}