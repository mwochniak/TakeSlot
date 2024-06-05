using LanguageExt.Common;
using TakeSlot.Application.Commands;
using TakeSlot.Application.Models.External;
using TakeSlot.Application.Results;

namespace TakeSlot.Application.Services;

public interface IAvailabilityOfFacilityService
{
    Task<Result<FacilityAvailabilityExternalDto>> GetTestFacilityWeeklyAvailabilityAsync(string mondayDate, CancellationToken cancellationToken);

    Task<Result<BaseResult>> TakeTestFacilitySlotAsync(TakeTestFacilitySlot facilitySlot, CancellationToken cancellationToken);
}