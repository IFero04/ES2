using System;
using System.Net.Http;
using System.Net.Http.Json;
using BusinessLogic.Entities;
using BusinessLogic.Models;

namespace BusinessLogic.Services
{
    public interface IInscricaoService
    {
        Task<bool> Inscrever(CreateInscricaoEventoModel model);
        Task<bool> RemoverInscricao(Guid id);
        Task<InscricaoEvento?> GetInscricaoByEventoParticipante(Guid idEvento, Guid idParticipante);
        Task<Guid?> GetInscricaoByEventoParticipanteId(Guid idEvento, Guid idParticipante);
        Task<bool> VerificarInscricaoEvento(Guid idEvento, Guid idParticipante);
        Task<GetInscricaoEvento?> GetEventosInscrito(Guid idParticipante);
    }

    public class ServiceInscricaoEvento : IInscricaoService
    {
        private readonly HttpClient _httpClient;
        private readonly IFeedbackService _feedbackService;
        
        public ServiceInscricaoEvento(HttpClient httpClient, IFeedbackService feedbackService)
        {
            _httpClient = httpClient;
            _feedbackService = feedbackService;
        }

        public async Task<bool> Inscrever(CreateInscricaoEventoModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5052/api/InscricaoEvento", model);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoverInscricao(Guid id)
        {
            Guid? feedbackId = await _feedbackService.GetFeedbackByInscricaoId(id);
            
            if (feedbackId != null)
            {
                bool feedbackRemovido = await _feedbackService.RemoverFeedback(feedbackId.Value);
                if (!feedbackRemovido)
                    return false;
            }
            try
            {
                var inscricaoRemovida = await _httpClient.DeleteAsync($"http://localhost:5052/api/InscricaoEvento/{id}");
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
            var feedback = await GetInscricaoByEventoParticipante(idEvento, idParticipante);

            return feedback?.Id;
        }
        
        public async Task<bool> VerificarInscricaoEvento(Guid idEvento, Guid idParticipante)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoEvento/CheckInscricao/{idEvento}/{idParticipante}");
            
            return response.IsSuccessStatusCode;
        }

        public async Task<GetInscricaoEvento?> GetEventosInscrito(Guid idParticipante)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/InscricaoEvento/GetEventosInscritos/{idParticipante}");
            
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<GetInscricaoEvento>();

            return null;
        }
    }
}