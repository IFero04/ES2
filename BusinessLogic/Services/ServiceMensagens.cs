using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Services
{
    public interface IMensagensService
    {
        Task<bool> VerificarMensagem(Guid idEvento);
        Task<List<Mensagem>?> GetMensagensByEvento(Guid idEvento);
        Task<List<Guid>?> GetMensagensIdByEvento(Guid idEvento);
        Task<bool> RemoverMensagem(Guid idMensagem);
    }

    public class ServiceMensagens : IMensagensService
    {
        private readonly HttpClient _httpClient;

        public ServiceMensagens(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerificarMensagem(Guid idEvento)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/Mensagem/CheckMensagemByEvento/{idEvento}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<bool>();

            return false;
        }

        public async Task<List<Mensagem>?> GetMensagensByEvento(Guid idEvento)
        {
            if (await VerificarMensagem(idEvento))
            {
                var response = await _httpClient.GetAsync($"http://localhost:5052/api/Mensagem/GetMensagensByEvento/{idEvento}");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<List<Mensagem>>();
            }

            return null;
        }

        public async Task<List<Guid>?> GetMensagensIdByEvento(Guid idEvento)
        {
            var response = await GetMensagensByEvento(idEvento);

            return response?.Select(r => r.Id).ToList();
        }

        public async Task<bool> RemoverMensagem(Guid idMensagem)
        {
            var mensagemRemovida = await _httpClient.DeleteAsync($"http://localhost:5052/api/Mensagem/{idMensagem}");

            return mensagemRemovida.IsSuccessStatusCode;
        }
    }
}
