using System.Net.Http.Json;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace BusinessLogic.Services;

public interface IAtividadeService
{
    Task<bool> VerificarAtividadeByEvento(Guid idEvento);
    Task<AtividadeDetalheModel[]?> GetAtividadeByEvento(Guid idEvento);
    Task<Guid[]?> GetAtividadeIdByEvento(Guid idEvento);
    Task<bool> CriarAtividade(CreateAtividadeModel model);
    Task<bool> RemoverAtividade(Guid idAtividade);

    Task<bool> EditarAtividade(Guid idAtividade, Atividade atividade);
}

public class ServiceAtividade : IAtividadeService
{
    private readonly HttpClient _httpClient;
    private readonly IInscricaoAtividadeService _inscricaoAtividadeService;

    public ServiceAtividade(HttpClient httpClient, IInscricaoAtividadeService inscricaoAtividadeService)
    {
        _httpClient = httpClient;
        _inscricaoAtividadeService = inscricaoAtividadeService;
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
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5052/api/Atividade", model);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoverAtividade(Guid idAtividade)
    {
        List<Guid>? idInscricoes = await _inscricaoAtividadeService.GetInscricaoByAtividadeId(idAtividade);

        if (idInscricoes != null && idInscricoes.Count() > 0)
        {
            foreach (var id in idInscricoes)
            {
                bool response = await _inscricaoAtividadeService.RemoverInscricao(id);

                if (!response) return false;
            }
        }
        
        var eleminado = await _httpClient.DeleteAsync($"http://localhost:5052/api/Atividade/{idAtividade}");

        return eleminado.IsSuccessStatusCode;
    }
    
    public async Task<bool> EditarAtividade(Guid idAtividade, Atividade atividade)
    {
        var antigaAtividade = await _httpClient.GetAsync($"http://localhost:5052/api/Atividade/{idAtividade}");

        if (antigaAtividade.IsSuccessStatusCode)
        {
            var var = await antigaAtividade.Content.ReadFromJsonAsync<Atividade>();

            atividade.IdEvento = var.IdEvento;
        }

        bool response = await RemoverAtividade(idAtividade);
        if (response)
        {
            CreateAtividadeModel model = new CreateAtividadeModel()
            {
                Nome = atividade.Nome,
                Data = atividade.Data,
                Hora = atividade.Hora,
                Descricao = atividade.Descricao,
                IdEvento = atividade.IdEvento
            };
            return await CriarAtividade(model);
        }

        return false;
    }
}