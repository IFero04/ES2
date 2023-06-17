namespace BusinessLogic.Models;

public class CreateFeedbackModel
{
    public string Comentario { get; set; } = null!;
    public Guid IdInscricao { get; set; }
}