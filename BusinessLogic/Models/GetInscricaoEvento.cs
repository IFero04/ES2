using BusinessLogic.Entities;

namespace BusinessLogic.Models;

public class GetInscricaoEvento
{
    public List<Guid>? IdEventos { get; set; }
    public List<IngressoDetalheModel> Ingressos { get; set; }
}