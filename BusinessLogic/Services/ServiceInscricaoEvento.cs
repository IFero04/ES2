using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Services
{
    public interface IInscricaoService
    {
        Task<bool> RemoverInscricao(Guid id);
        Task<InscricaoEvento?> GetInscricaoByEventoParticipante(Guid idEvento, Guid idParticipante);
        Task<Guid?> GetInscricaoByEventoParticipanteId(Guid idEvento, Guid idParticipante);
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
    }
}