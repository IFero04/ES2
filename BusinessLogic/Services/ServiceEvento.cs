namespace BusinessLogic.Services;

using BusinessLogic.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessLogic.Entities;

public interface IEventoService
{
    Task<EventoDetailsModel[]?> GetEventos(); 
    Task<EventoDetailsModel?> GetDetalhesEvento(Guid idEvento);
    Task<bool> RemoverEvento(Guid idEvento);
}


public class ServiceEvento : IEventoService
{
    private readonly HttpClient _httpClient;
    private readonly IAtividadeService _atividadeService;
    private readonly IInscricaoEventoService _inscricaoEventoService;
    private readonly IIngressosService _ingressosService;
   
    public ServiceEvento(HttpClient httpClient, IAtividadeService atividadeService, IInscricaoEventoService inscricaoEventoService, IIngressosService ingressosService)
    {
        _httpClient = httpClient;
        _atividadeService = atividadeService;
        _inscricaoEventoService = inscricaoEventoService;
        _ingressosService = ingressosService;
    }

    public async Task<EventoDetailsModel[]?> GetEventos()
    {
        var response = await _httpClient.GetAsync("http://localhost:5052/api/Evento");

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<EventoDetailsModel[]>();

        return null;
    }
    
    public async Task<EventoDetailsModel?> GetDetalhesEvento(Guid idEvento)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5052/api/Evento/Detalhe/{idEvento}");

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<EventoDetailsModel>();

        return null;
    }

    public async Task<bool> RemoverEvento(Guid idEvento)
    {
        Guid[]? idAtividades = await _atividadeService.GetAtividadeIdByEvento(idEvento);

        if (idAtividades != null && idAtividades.Length > 0)
        {
            foreach (var id in idAtividades)
            {
                bool check = await _atividadeService.RemoverAtividade(id);
                if (!check) return false;
            }
        }
        
        Console.WriteLine("Passo1");
        
        List<Guid>? idIncricoes = await _inscricaoEventoService.GetInscricaoIdByEvento(idEvento);

        if (idIncricoes != null && idIncricoes.Count > 0)
        {
            foreach (var id in idIncricoes)
            {
                bool check = await _inscricaoEventoService.RemoverInscricao(id);
                if (!check) return false;
            }
        }
        
        Console.WriteLine("Passo2");
        
        List<Guid>? idIngressos = await _ingressosService.GetIngressoIdByEvento(idEvento);
        
        if (idIngressos != null && idIngressos.Count > 0)
        {
            foreach (var id in idIngressos)
            {
                bool check = await _ingressosService.RemoverIngresso(id);
                if (!check) return false;
            }
        }
        
        Console.WriteLine("Passo3");
        
        var eventoRemovido = await _httpClient.DeleteAsync($"http://localhost:5052/api/Evento/{idEvento}");
                
        return eventoRemovido.IsSuccessStatusCode;
    }
}