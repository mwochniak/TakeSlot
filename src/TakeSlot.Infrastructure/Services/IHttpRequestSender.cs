using LanguageExt.Common;
using TakeSlot.Application.Results;

namespace TakeSlot.Infrastructure.Services;

internal interface IHttpRequestSender
{
    Task<Result<T>> SendRequestAsync<T>(HttpClient httpClient, HttpRequestMessage message, CancellationToken cancellationToken);

    Task<Result<BaseResult>> SendRequestAsync(HttpClient httpClient, HttpRequestMessage message,
        CancellationToken cancellationToken);
}