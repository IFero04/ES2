using System.Net.Http.Json;
using BusinessLogic.Models;

namespace BusinessLogic.Services;

public interface IAtividadeService
{
    Task<bool> VerificarAtividadeByEvento(Guid idEvento);
    Task<AtividadeDetalheModel[]?> GetAtividadeByEvento(Guid idEvento);
    Task<Guid[]?> GetAtividadeIdByEvento(Guid idEvento);
    Task<bool> CriarAtividade(CreateAtividadeModel model);
}

public class ServiceAtividade : IAtividadeService
{
    private readonly HttpClient _httpClient;

    public ServiceAtividade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> VerificarAtividadeByEvento(Guid idEvento)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5052/api/Atividade/CheckAtividadesByEvento/{idEvento}");
            
        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();

        return false;
    }
    
    public async Task<AtividadeDetalheModel[]?> GetAtividadeByEvento(Guid idEvento)
    {
        if (await VerificarAtividadeByEvento(idEvento))
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/Atividade/GetAtividadesByEvento/{idEvento}");

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<AtividadeDetalheModel[]>();
        
            return null;
        }

        return null;
    }

    public async Task<Guid[]?> GetAtividadeIdByEvento(Guid idEvento)
    {
        var response = await GetAtividadeByEvento(idEvento);

        return response?.Select(r => r.Id).ToArray();
    }

    public async Task<bool> CriarAtividade(CreateAtividadeModel model)
    {
        

        return false;
    }
}