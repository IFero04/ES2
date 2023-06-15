using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Utilizador
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public string? Telefone { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual ICollection<InscricaoAtividade> InscricaoAtividades { get; set; } = new List<InscricaoAtividade>();

    public virtual ICollection<InscricaoEvento> InscricaoEventos { get; set; } = new List<InscricaoEvento>();
}
