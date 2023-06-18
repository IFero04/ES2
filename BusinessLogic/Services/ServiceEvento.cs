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
    Task<EventoDetailsModel?> GetDetalhesEvento(Guid idEvento);
}


public class ServiceEvento : IEventoService
{
    private readonly HttpClient _httpClient;

    public ServiceEvento(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EventoDetailsModel?> GetDetalhesEvento(Guid idEvento)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5052/api/Evento/Detalhe/{idEvento}");

        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<EventoDetailsModel>();

        return null;
    }
}