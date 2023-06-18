using System.Net;
using System.Net.Http.Json;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace BusinessLogic.Services;

public interface IInscricaoAtividadeService
{
    Task<bool> Inscrever(CreateInscricaoAtividadeModel model);
    Task<bool> VerificarInscricaoAtividade(Guid idAtividade, Guid idParticipante);
    Task<bool> RemoverInscricao(Guid id);
    Task<InscricaoAtividade?> GetInscricaoByAtividade(Guid idAtividade);
    Task<Guid?> GetInscricaoByAtividadeID(Guid idAtividade);
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
        
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();

        return false;
    }

    public async Task<bool> RemoverInscricao(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"http://localhost:5052/api/InscricaoAtividade/{id}");

        return response.IsSuccessStatusCode;
    }

    public async Task<InscricaoAtividade?> GetInscricaoByAtividade(Guid idAtividade)
    {
        try
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/IncricaoAtividade/ByAtividade/{idAtividade}");

            if (response.IsSuccessStatusCode)
            {
                var inscricaoAtividade = await response.Content.ReadFromJsonAsync<InscricaoAtividade>();
                return inscricaoAtividade;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Guid?> GetInscricaoByAtividadeID(Guid idAtividade)
    {
        var inscricaoAtividade = await GetInscricaoByAtividade(idAtividade);

        return inscricaoAtividade?.Id;
    }
}