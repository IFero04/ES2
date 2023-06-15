using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Mensagem
{
    public Guid Id { get; set; }

    public string Mensagem1 { get; set; } = null!;

    public Guid IdEvento { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;
}
