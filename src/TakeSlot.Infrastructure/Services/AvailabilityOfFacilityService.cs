using System.Text;
using System.Text.Json;
using LanguageExt.Common;
using TakeSlot.Application.Clients;
using TakeSlot.Application.Commands;
using TakeSlot.Application.Models.External;
using TakeSlot.Application.Results;
using TakeSlot.Application.Services;

namespace TakeSlot.Infrastructure.Services;

internal sealed class AvailabilityOfFacilityService(ISlotServiceApiClient apiClient, IHttpRequestSender httpRequestSender) : IAvailabilityOfFacilityService
{
    private const string GetTestSpecialistWeeklyAvailabilityUri = "/api/availability/GetWeeklyAvailability";
    private const string TakeTestSpecialistSlotUri = "/api/availability/TakeSlot";
    
    public async Task<Result<FacilityAvailabilityExternalDto>> GetTestFacilityWeeklyAvailabilityAsync(string mondayDate, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"{GetTestSpecialistWeeklyAvailabilityUri}/{mondayDate}", UriKind.Relative),
            Method = HttpMethod.Get
        };
        
        var responseResult = await httpRequestSender.SendRequestAsync<FacilityAvailabilityExternalDto>(await apiClient.GetClientAsync(), request, cancellationToken);
        return responseResult.Match(success => success, exception => new Result<FacilityAvailabilityExternalDto>(exception));
    }

    public async Task<Result<BaseResult>> TakeTestFacilitySlotAsync(TakeTestFacilitySlot facilitySlot, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(TakeTestSpecialistSlotUri, UriKind.Relative),
            Method = HttpMethod.Post,
            Content = new StringContent(JsonSerializer.Serialize(facilitySlot), Encoding.UTF8, "application/json")
        };
        
        var responseResult = await httpRequestSender.SendRequestAsync(await apiClient.GetClientAsync(), request, cancellationToken);
        return responseResult.Match(succ => new Result<BaseResult>(new BaseResult()), ex => new Result<BaseResult>(ex));
    }
}