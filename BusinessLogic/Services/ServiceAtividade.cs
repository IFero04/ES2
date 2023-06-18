namespace BusinessLogic.Services;

public interface IAtividadeService
{
}

public class ServiceAtividade : IAtividadeService
{
    private readonly HttpClient _httpClient;

    public ServiceAtividade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}