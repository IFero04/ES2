using System.Text.Json.Serialization;

namespace BusinessLogic.Models;

public class CreateEventModel
{
    public string Nome { get; set; } = null!;
    public DateOnly Data { get; set; }
    public string Hora { get; set; } = null!;
    public string Local { get; set; } = null!;
    public string? Descricao { get; set; }
    public string Categoria { get; set; } = null!;
    public int Capacidade { get; set; }
    public Guid IdOrganizador { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CreateIngressoModel>? Ingressos { get; set; }
}