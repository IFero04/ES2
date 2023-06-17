﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Services;

public interface IFeedbackService
{
    Task<bool> RemoverFeedback(Guid id);
    Task<Feedback?> GetFeedbackByInscricao(Guid idInscricao);
    Task<Guid?> GetFeedbackByInscricaoId(Guid idInscricao);
}

public class ServiceFeedback : IFeedbackService
{
    private readonly HttpClient _httpClient;

    public ServiceFeedback(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> RemoverFeedback(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5052/api/Feedback/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Feedback?> GetFeedbackByInscricao(Guid idInscricao)
    {
        try
        {
            var response = await _httpClient.GetAsync($"http://localhost:5052/api/Feedback/ByInscricao/{idInscricao}");

            if (response.IsSuccessStatusCode)
            {
                var feedback = await response.Content.ReadFromJsonAsync<Feedback>();
                return feedback;
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Guid?> GetFeedbackByInscricaoId(Guid idInscricao)
    {
        var feedback = await GetFeedbackByInscricao(idInscricao);

        return feedback?.Id;
    }
}