using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class InscricaoAtividade
{
    public Guid Id { get; set; }

    public Guid IdAtividade { get; set; }

    public Guid IdParticipante { get; set; }

    public virtual Atividade IdAtividadeNavigation { get; set; } = null!;

    public virtual Utilizador IdParticipanteNavigation { get; set; } = null!;
}
