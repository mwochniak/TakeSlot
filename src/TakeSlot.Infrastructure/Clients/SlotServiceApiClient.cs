using TakeSlot.Application.Clients;

namespace TakeSlot.Infrastructure.Clients;

internal sealed class SlotServiceApiClient(ISlotServiceAuthenticator authenticator, HttpClient httpClient) : ISlotServiceApiClient
{
    public async Task<HttpClient> GetClientAsync()
    {
        httpClient.DefaultRequestHeaders.Authorization = await authenticator.GetAuthenticationHeaderAsync();
        return httpClient;
    }
}