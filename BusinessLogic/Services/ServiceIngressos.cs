using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Services
{
    public interface IIngressosService
    {
        Task<bool> VerificarIngresso(Guid idEvento);
        Task<List<Ingresso>?> GetIngressoByEvento(Guid idEvento);
        Task<List<Guid>?> GetIngressoIdByEvento(Guid idEvento);
        Task<bool> RemoverIngresso(Guid idIngresso);
    }
    
    public class ServiceIngressos : IIngressosService
    {
        private readonly HttpClient _httpClient;

        public ServiceIngressos(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerificarIngresso(Guid idEvento)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/Ingresso/CheckIngressoByEvento/{idEvento}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<bool>();

            return false;
        }

        public async Task<List<Ingresso>?> GetIngressoByEvento(Guid idEvento)
        {
            if (await VerificarIngresso(idEvento))
            {
                var response = await _httpClient.GetAsync($"http://localhost:5052/api/Ingresso/GetIngressoByEvento/{idEvento}");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<List<Ingresso>>();
            }

            return null;
        }

        public async Task<List<Guid>?> GetIngressoIdByEvento(Guid idEvento)
        {
            var response = await GetIngressoByEvento(idEvento);

            return response?.Select(r => r.Id).ToList();
        }

        public async Task<bool> RemoverIngresso(Guid idIngresso)
        {
            var ingressoRemovida = await _httpClient.DeleteAsync($"http://localhost:5052/api/Ingresso/{idIngresso}");
                
            return ingressoRemovida.IsSuccessStatusCode;
        }
    }
}