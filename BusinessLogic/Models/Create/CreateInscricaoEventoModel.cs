namespace BusinessLogic.Models;

public class CreateInscricaoEventoModel
{
    public Guid IdEvento { get; set; }
    public Guid IdParticipante { get; set; }
    public Guid TipoIngresso { get; set; }
}