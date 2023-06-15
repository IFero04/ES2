using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Feedback
{
    public Guid Id { get; set; }

    public string Comentario { get; set; } = null!;

    public Guid IdInscricao { get; set; }

    public virtual InscricaoEvento IdInscricaoNavigation { get; set; } = null!;
}
