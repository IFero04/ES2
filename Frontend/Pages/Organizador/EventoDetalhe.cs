﻿@page "/evento-detalhes/{id}"
@using System.ComponentModel.DataAnnotations
@using System.IdentityModel.Tokens.Jwt
@using BusinessLogic.Models
@inject HttpClient Http
@inject Microsoft.JSInterop.IJSRuntime JSRuntime

<h3>Detalhes do Evento</h3>

@if (evento == null)
{
    <p>
        <em>Evento não encontrado... @Id</em>
    </p>
}
else
{
    <div class="evento-details">
        <h4>@evento.Nome</h4>
        <p>Data: @evento.Data.ToShortDateString()</p>
        <p>Hora: @evento.Hora</p>
        <p>Local: @evento.Local</p>
        <p>Categoria: @evento.Categoria</p>
        <p>Capacidade: @evento.Capacidade</p>
        <p>Número de Atividades: @evento.NumeroAtidades</p>
        <p>Ingresso Barato: @evento.IngressoBarato</p>
    </div>

    <h4>Atividades do Evento</h4>
@if (evento.Atividades != null && evento.Atividades.Any())
{
    <ul>
        @foreach (var atividade in evento.Atividades)
        {
            <h5>@atividade.Nome</h5>
            <li>@atividade.Data</li>
            <li>@atividade.Hora</li>
            <li>@atividade.Descricao</li>
            
            @if (jaInscrito)
            {
                <button @onclick="() => RegistrarAtividade(atividade.Id)">Registrar-se na Atividade</button>
            }
        }
    </ul>
}
else
{
    <p>
        <em>Nenhuma atividade encontrada para este evento.</em>
    </p>
}

    <h4>Escolha o Ingresso</h4>
    <select @bind="selectedIngresso">
        <option value="">Selecione um ingresso</option>
        @foreach (var ingresso in evento.Ingressos)
        {
            <option value="@ingresso.Id">@ingresso.Nome - @ingresso.Preco</option>
        }
    </select>

    <button @onclick="RegistrarEvento" disabled="@string.IsNullOrEmpty(selectedIngresso) ||@jaInscrito">Registrar-se no Evento</button>
}

@code {
    private EventoDetailsModel evento;
    private string selectedIngresso;
    private bool jaInscrito;
    
    private async Task RegistrarEvento()
    {
        var model = new CreateInscricaoEventoModel()
        {
            IdParticipante = Guid.Parse(_tokenClaims["idUtilizador"]),
            IdEvento = Guid.Parse(Id),
            TipoIngresso = Guid.Parse(selectedIngresso)
        };

        var response = await Http.PostAsJsonAsync("http://localhost:5052/api/InscricaoEvento", model);
        
        if (response.IsSuccessStatusCode )
        {
            jaInscrito = true;
        }
    }
    
    private async Task RegistrarAtividade(Guid atividadeId)
    {
        var inscrito = await VerificarInscricaoAtividade(atividadeId, Guid.Parse(_tokenClaims["idUtilizador"]));
        
        if(inscrito)
            return;
        
        var model = new CreateInscricaoAtividadeModel()
        {
            IdParticipante = Guid.Parse(_tokenClaims["idUtilizador"]),
            IdAtividade = atividadeId
        };
        
        var response = await Http.PostAsJsonAsync("http://localhost:5052/api/InscricaoAtividade", model);
        
        if (response.IsSuccessStatusCode )
        {
            Console.WriteLine("Sucesso");
        }
    }
    
    
    [Parameter]
    public string Id { get; set; }

    protected async Task HandleEvento()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            var idPGuid = Guid.Parse(Id);
            evento = await Http.GetFromJsonAsync<EventoDetailsModel>($"http://localhost:5052/api/Evento/Detalhe/{idPGuid}");
        }
    }
    [Inject]
    private NavigationManager? NavigationManager { get; set; }
    private Dictionary<string, string> _tokenClaims = new Dictionary<string, string>();
    protected override async Task OnInitializedAsync()
    {
        var token = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "token");

        if (!string.IsNullOrEmpty(token))
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var decodedToken = jwtHandler.ReadJwtToken(token);

            _tokenClaims = decodedToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            
            if (_tokenClaims["role"] != "Participante" && _tokenClaims["role"] != "Organizador")
            {
                NavigationManager?.NavigateTo("/login");
            }
        }
        else
        {
            NavigationManager?.NavigateTo("/login");
        }
        await HandleEvento();
        jaInscrito = await VerificarInscricaoEvento(Guid.Parse(Id), Guid.Parse(_tokenClaims["idUtilizador"]));
    }

    private async Task<bool> VerificarInscricaoEvento(Guid idEvento, Guid idParticipante)
    {
        var response = await Http.GetAsync($"http://localhost:5052/api/InscricaoEvento/CheckInscricao/{idEvento}/{idParticipante}");
        return response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>();
    }
    
    private async Task<bool> VerificarInscricaoAtividade(Guid idAtividade, Guid idParticipante)
    {
        var response = await Http.GetAsync($"http://localhost:5052/api/InscricaoAtividade/CheckAtividade/{idAtividade}/{idParticipante}");
        return response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>();
    } 
}

<style>
    .evento-details {
        border: 1px solid #ccc;
        padding: 10px;
    }
</style>
