namespace TakeSlot.Application.Clients;

public interface ISlotServiceApiClient
{
    Task<HttpClient> GetClientAsync();
}