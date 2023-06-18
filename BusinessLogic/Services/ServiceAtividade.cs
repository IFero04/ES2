namespace BusinessLogic.Services;

public interface IAtividadeService
{
    Task<bool> RemoverAtividade(Guid id);
}

public class ServiceAtividade : IAtividadeService
{
    _httpClient = httpClient;
}