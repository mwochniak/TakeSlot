using System.Net.Http.Headers;

namespace TakeSlot.Infrastructure.Clients;

internal interface ISlotServiceAuthenticator
{
    Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync();
}