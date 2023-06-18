using System.Net;
using System.Net.Http.Json;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace BusinessLogic.Services;

public interface IInscricaoAtividadeService
{
    Task<bool> Inscrever(CreateInscricaoAtividadeModel model);
    Task<bool> RemoverInscricao(Guid id);
    Task<bool> VerificartInscricaoByAtividadeParticipante(Guid idAtividade, Guid idParticipante);
    Task<InscricaoAtividade?> GetInscricaoByAtividadeParticipante(Guid idAtividade, Guid idParticipante);
    Task<Guid?> GetInscricaoByAtividadeParticipanteId(Guid idAtividade, Guid idParticipante);
    Task<bool> VerificarInscricaoByAtividade(Guid idAtividade);
    Task<InscricaoAtividade[]?> GetInscricaoByAtividade(Guid idAtividade);
    Task<Guid[]?> GetInscricaoByAtividadeId(Guid idAtividade);
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

    public async Task<bool> RemoverInscricao(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"http://localhost:5052/api/InscricaoAtividade/{id}");

        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> VerificartInscricaoByAtividadeParticipante(Guid idAtividade, Guid idParticipante)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoAtividade/CheckInscricao/{idAtividade}/{idParticipante}");
        
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();

        return false;
    }

    public async Task<InscricaoAtividade?> GetInscricaoByAtividadeParticipante(Guid idAtividade, Guid idParticipante)
    {
        try
        {
            if (await VerificartInscricaoByAtividadeParticipante(idAtividade, idParticipante))
            {
                var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoAtividade/GetByAtividadeParticipante/{idAtividade}/{idParticipante}");
                
                if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<InscricaoAtividade>();
                
                return null;
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Guid?> GetInscricaoByAtividadeParticipanteId(Guid idAtividade, Guid idParticipante)
    {
        var response = await GetInscricaoByAtividadeParticipante(idAtividade, idParticipante);

        return response?.Id;
    }
    
    public async Task<bool> VerificarInscricaoByAtividade(Guid idAtividade)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5052/api/IncricaoAtividade/CheckInscricaoByAtividade/{idAtividade}");
            
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();

        return false;
    }

    public async Task<InscricaoAtividade[]?> GetInscricaoByAtividade(Guid idAtividade)
    {
        try
        {
            if (await VerificarInscricaoByAtividade(idAtividade))
            {
                var response = await _httpClient.GetAsync($"http://localhost:5052/api/IncricaoAtividade/ByAtividade/{idAtividade}");

                if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<InscricaoAtividade[]>();
                
                return null;
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Guid[]?> GetInscricaoByAtividadeId(Guid idAtividade)
    {
        var inscricaoAtividade = await GetInscricaoByAtividade(idAtividade);
        
        return inscricaoAtividade?.Select(i => i.Id).ToArray();
    }
}