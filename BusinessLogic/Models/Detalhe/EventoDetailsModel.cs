using BusinessLogic.Entities;

namespace BusinessLogic.Models;

public class EventoDetailsModel
{
    public Guid Id { get; set; }
    
    public string Nome { get; set; } = null!;

    public DateOnly Data { get; set; }

    public string Hora { get; set; } = null!;

    public string Local { get; set; } = null!;

    public string? Descricao { get; set; }

    public string? Categoria { get; set; } = null!;

    public int Capacidade { get; set; }
    
    public UtilizadorDetalheModel? Organizador { get; set; }

    public string NomeOrganizador => Organizador?.Nome!;
    
    public List<IngressoDetalheModel>? Ingressos { get; set; }
    
    public List<AtividadeDetalheModel>? Atividades { get; set; }

    public int NumeroAtidades => Atividades?.Count ?? 0;

    public double IngressoBarato
    {
        get
        {
            if (Ingressos != null && Ingressos.Count > 0)
            {
                return Ingressos.Min(ingresso => ingresso.Preco);
            }

            return 0.0;
        }
    }

    public int ParticipantesInscritos => Ingressos?.Count ?? 0;

}