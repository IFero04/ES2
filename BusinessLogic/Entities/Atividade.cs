using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Atividade
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public DateOnly Data { get; set; }

    public string Hora { get; set; } = null!;

    public string? Descricao { get; set; }

    public Guid IdEvento { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual ICollection<InscricaoAtividade> InscricaoAtividades { get; set; } = new List<InscricaoAtividade>();
}
