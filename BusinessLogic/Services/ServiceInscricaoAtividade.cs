using System.Net.Http.Json;
using BusinessLogic.Models;

namespace BusinessLogic.Services;

public interface IInscricaoAtividadeService
{
    Task<bool> Inscrever(CreateInscricaoAtividadeModel model);
    Task<bool> VerificarInscricaoAtividade(Guid idAtividade, Guid idParticipante);
}

public class ServiceInscricaoAtividade : IInscricaoAtividadeService
{
    private readonly HttpClient _httpClient;

    public ServiceInscricaoAtividade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Inscrever(CreateInscricaoAtividadeModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5052/api/InscricaoAtividade", model);

        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> VerificarInscricaoAtividade(Guid idAtividade, Guid idParticipante)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoAtividade/CheckInscricao/{idAtividade}/{idParticipante}");
        
        return response.IsSuccessStatusCode;
    } 
    
}