namespace Frontend.Pages.Organizador;

public class CriarAtividade
{
       private async Task RemoverAtividade(Guid idAtividade)
    {
        Console.WriteLine("Passo1");
        
        await ServiceAtividade.RemoverAtividade(idAtividade);
        
        var idPGuid = Guid.Parse(Id);

        evento = await Http.GetFromJsonAsync<EventoDetailsModel>($"http://localhost:5052/api/Evento/Detalhe/{idPGuid}");
    }
}