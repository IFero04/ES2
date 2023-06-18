using System;
using System.Net.Http;
using System.Net.Http.Json;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface IInscricaoEventoService
    {
        Task<bool> Inscrever(CreateInscricaoEventoModel model);
        Task<bool> RemoverInscricao(Guid idInscricao);
        Task<bool> VerificarInscricao(Guid idEvento);
        Task<List<InscricaoEvento>?> GetInscricaoByEvento(Guid idEvento);
        Task<List<Guid>?> GetInscricaoIdByEvento(Guid idEvento);
        Task<bool> RemoverInscricaoEspc(Guid idInscricao, Guid idEvento, Guid idParticipante);
        Task<InscricaoEvento?> GetInscricaoByEventoParticipante(Guid idEvento, Guid idParticipante);
        Task<Guid?> GetInscricaoByEventoParticipanteId(Guid idEvento, Guid idParticipante);
        Task<bool> VerificarInscricaoEvento(Guid idEvento, Guid idParticipante);
        Task<GetInscricaoEvento?> GetEventosInscrito(Guid idParticipante);
    }

    public class ServiceInscricaoEvento : IInscricaoEventoService
    {
        private readonly HttpClient _httpClient;
        private readonly IFeedbackService _feedbackService;
        private readonly IAtividadeService _atividadesService;
        private readonly IInscricaoAtividadeService _inscricaoAtividadeService;
        
        public ServiceInscricaoEvento(HttpClient httpClient, IFeedbackService feedbackService, IAtividadeService atividadesService, IInscricaoAtividadeService inscricaoAtividadeService)
        {
            _httpClient = httpClient;
            _feedbackService = feedbackService;
            _atividadesService = atividadesService;
            _inscricaoAtividadeService = inscricaoAtividadeService;
        }

        public async Task<bool> Inscrever(CreateInscricaoEventoModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5052/api/InscricaoEvento", model);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoverInscricao(Guid idInscricao)
        {
            Guid? feedbackId = await _feedbackService.GetFeedbackByInscricaoId(idInscricao);
            
            if (feedbackId != null)
            {
                bool removido = await _feedbackService.RemoverFeedback(feedbackId.Value);
                if (!removido)
                    return false;
            }

            try
            {
                var inscricaoRemovida = await _httpClient.DeleteAsync($"http://localhost:5052/api/InscricaoEvento/{idInscricao}");
                
                return inscricaoRemovida.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> VerificarInscricao(Guid idEvento)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoEvento/CheckInscricaoByEvento/{idEvento}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<bool>();

            return false;
        }

        public async Task<List<InscricaoEvento>?> GetInscricaoByEvento(Guid idEvento)
        {
            if (await VerificarInscricao(idEvento))
            {
                var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoEvento/GetInscricaoByEvento/{idEvento}");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<List<InscricaoEvento>>();
            }

            return null;
        }

        public async Task<List<Guid>?> GetInscricaoIdByEvento(Guid idEvento)
        {
            var response = await GetInscricaoByEvento(idEvento);

            return response?.Select(r => r.Id).ToList();
        }
        
        public async Task<bool> RemoverInscricaoEspc(Guid idInscricao, Guid idEvento, Guid idParticipante)
        {
            Guid? feedbackId = await _feedbackService.GetFeedbackByInscricaoId(idInscricao);
            
            if (feedbackId != null)
            {
                bool removido = await _feedbackService.RemoverFeedback(feedbackId.Value);
                if (!removido)
                    return false;
            }

            Guid[]? atividadesId = await _atividadesService.GetAtividadeIdByEvento(idEvento);

            if (atividadesId != null)
            {
                foreach (var idAtividade in atividadesId)
                {
                    Guid? idInscricaoAtividade = await _inscricaoAtividadeService.GetInscricaoByAtividadeParticipanteId(idAtividade, idParticipante);
                    if (idInscricaoAtividade != null)
                    {
                        bool removido = await _inscricaoAtividadeService.RemoverInscricao(idInscricaoAtividade.Value);
                        if (!removido) return false;
                    }
                }
            }
            
            try
            {
                var inscricaoRemovida = await _httpClient.DeleteAsync($"http://localhost:5052/api/InscricaoEvento/{idInscricao}");
                return inscricaoRemovida.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<InscricaoEvento?> GetInscricaoByEventoParticipante(Guid idEvento, Guid idParticipante)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoEvento/GetByEventoParticipante/{idEvento}/{idParticipante}");
                if (response.IsSuccessStatusCode)
                {
                    var inscricao = await response.Content.ReadFromJsonAsync<InscricaoEvento>();
                    return inscricao;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public async Task<Guid?> GetInscricaoByEventoParticipanteId(Guid idEvento, Guid idParticipante)
        {
            var response = await GetInscricaoByEventoParticipante(idEvento, idParticipante);

            return response?.Id;
        }
        
        public async Task<bool> VerificarInscricaoEvento(Guid idEvento, Guid idParticipante)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoEvento/CheckInscricao/{idEvento}/{idParticipante}");

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();

            return false;
        }

        public async Task<GetInscricaoEvento?> GetEventosInscrito(Guid idParticipante)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoEvento/GetEventosInscritos/{idParticipante}");
            
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<GetInscricaoEvento>();

            return null;
        }
    }
}