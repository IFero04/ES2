using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class InscricaoEvento
{
    public Guid Id { get; set; }

    public Guid IdEvento { get; set; }

    public Guid IdParticipante { get; set; }

    public Guid TipoIngresso { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual Utilizador IdParticipanteNavigation { get; set; } = null!;

    public virtual Ingresso TipoIngressoNavigation { get; set; } = null!;
}
