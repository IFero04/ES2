using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Ingresso
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public double Preco { get; set; }

    public int Quantidade { get; set; }

    public Guid IdEvento { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual ICollection<InscricaoEvento> InscricaoEventos { get; set; } = new List<InscricaoEvento>();
}
