using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Evento
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public DateOnly Data { get; set; }

    public string Hora { get; set; } = null!;

    public string Local { get; set; } = null!;

    public string? Descricao { get; set; }

    public string? Categoria { get; set; } = null!;

    public int Capacidade { get; set; }

    public Guid IdOrganizador { get; set; }

    public virtual ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();

    public virtual Utilizador IdOrganizadorNavigation { get; set; } = null!;

    public virtual ICollection<Ingresso> Ingressos { get; set; } = new List<Ingresso>();

    public virtual ICollection<InscricaoEvento> InscricaoEventos { get; set; } = new List<InscricaoEvento>();

    public virtual ICollection<Mensagem> Mensagems { get; set; } = new List<Mensagem>();
}
