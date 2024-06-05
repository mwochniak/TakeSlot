using System.Text.Json;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using TakeSlot.Application.Exceptions;
using TakeSlot.Application.Results;

namespace TakeSlot.Infrastructure.Services;

internal sealed class HttpRequestSender(ILogger<HttpRequestSender> logger) : IHttpRequestSender
{
    public async Task<Result<T>> SendRequestAsync<T>(HttpClient httpClient, HttpRequestMessage message, CancellationToken cancellationToken)
    {
        var response = await httpClient.SendAsync(message, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Request sent failed: {responseContent}", responseContent);
            var exception = new TakeSlotException(
                $"Request sent to uri : {message.RequestUri} has failed. Request content: {responseContent}. Response code: {response.StatusCode}");
            return new Result<T>(exception);
        }

        var deserializedResponse = JsonSerializer.Deserialize<T?>(responseContent);

        if (deserializedResponse is null)
        {
            var exception = new TakeSlotException(
                $"Cannot deserialize {nameof(T)}");
            return new Result<T>(exception);
        }

        return new Result<T>(deserializedResponse);
    }
    
    public async Task<Result<BaseResult>> SendRequestAsync(HttpClient httpClient, HttpRequestMessage message, CancellationToken cancellationToken)
    {
        var response = await httpClient.SendAsync(message, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Request sent failed: {responseContent}", responseContent);
            var exception = new TakeSlotException(
                $"Request sent to uri : {message.RequestUri} has failed. Request content: {responseContent}. Response code: {response.StatusCode}");
            return new Result<BaseResult>(exception);
        }

        return new Result<BaseResult>(new BaseResult());
    }
}