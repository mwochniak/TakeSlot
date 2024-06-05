using System.Net.Http.Headers;
using TakeSlot.Application.Options;

namespace TakeSlot.Infrastructure.Clients;

internal sealed class SlotServiceBasicAuthenticator(SlotServiceApiClientOptions options) : ISlotServiceAuthenticator
{
    public Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync()
    {
        var secret = $"{options.Username}:{options.Password}";
        var secret64Encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(secret));

        return Task.FromResult(new AuthenticationHeaderValue(
            "Basic",
            secret64Encoded
        ));
    }
}