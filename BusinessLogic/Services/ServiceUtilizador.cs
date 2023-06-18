using System.Net.Http.Json;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace BusinessLogic.Services;

public interface IUtilizadorService
{
    Task<bool> EditarInfo(Guid idUtilizador, Utilizador utilizador);
    Task<Utilizador> GetUtilizadorById(Guid idUtilizador);
}

public class ServiceUtilizador : IUtilizadorService
{
    private readonly HttpClient _httpClient;

    public ServiceUtilizador(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> EditarInfo(Guid idUtilizador, Utilizador utilizador)
    {
        
       
        var response = await _httpClient.PutAsJsonAsync($"http://localhost:5052/api/Utilizador/{idUtilizador}", utilizador);
        
        return response.IsSuccessStatusCode;
    }

    public async Task<Utilizador> GetUtilizadorById(Guid id)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5052/api/Utilizador/{id}");

        if (response.IsSuccessStatusCode)
        {
            
            var utilizador = await response.Content.ReadFromJsonAsync<Utilizador>();
            return utilizador;
        }
            
        return null;
    }
}