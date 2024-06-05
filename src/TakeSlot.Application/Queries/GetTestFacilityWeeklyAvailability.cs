using System.Globalization;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using TakeSlot.Application.Exceptions;
using TakeSlot.Application.Models;
using TakeSlot.Application.Models.External;
using TakeSlot.Application.Services;

namespace TakeSlot.Application.Queries;

public record GetTestFacilityWeeklyAvailability(string MondayDate) : IRequest<Result<FacilityAvailabilityDto>>;

internal sealed class GetTestFacilityAvailabilityHandler(IAvailabilityOfFacilityService availabilityOfFacilityService) : IRequestHandler<GetTestFacilityWeeklyAvailability, Result<FacilityAvailabilityDto>>
{
    public async Task<Result<FacilityAvailabilityDto>> Handle(GetTestFacilityWeeklyAvailability request, CancellationToken cancellationToken)
    {
        var mondayDateTime = DateTime.ParseExact(request.MondayDate, "yyyyMMdd", CultureInfo.InvariantCulture);

        if (mondayDateTime.DayOfWeek is not DayOfWeek.Monday)
        {
            return new Result<FacilityAvailabilityDto>(new TakeSlotValidationException(new List<string>()
                {"Passed date is not monday"}));
        }
        
        var availabilityResult = await availabilityOfFacilityService.GetTestFacilityWeeklyAvailabilityAsync(request.MondayDate, cancellationToken);

        return availabilityResult.Match(succ => Map(mondayDateTime, succ), ex => new Result<FacilityAvailabilityDto>(ex));
    }

    private FacilityAvailabilityDto Map(DateTime mondayDateTime, FacilityAvailabilityExternalDto availabilityExternal)
    {
        var list = new List<Slot>();
        list.AddRange(GetDaySlots(mondayDateTime, availabilityExternal.Monday,
            availabilityExternal.SlotDurationMinutes));
        list.AddRange(GetDaySlots(mondayDateTime.AddDays(1), availabilityExternal.Tuesday,
            availabilityExternal.SlotDurationMinutes));
        list.AddRange(GetDaySlots(mondayDateTime.AddDays(2), availabilityExternal.Wednesday,
            availabilityExternal.SlotDurationMinutes));
        list.AddRange(GetDaySlots(mondayDateTime.AddDays(3), availabilityExternal.Thursday,
            availabilityExternal.SlotDurationMinutes));
        list.AddRange(GetDaySlots(mondayDateTime.AddDays(4), availabilityExternal.Friday,
            availabilityExternal.SlotDurationMinutes));
        list.AddRange(GetDaySlots(mondayDateTime.AddDays(5), availabilityExternal.Saturday,
            availabilityExternal.SlotDurationMinutes));
        list.AddRange(GetDaySlots(mondayDateTime.AddDays(6), availabilityExternal.Sunday,
            availabilityExternal.SlotDurationMinutes));

        return new FacilityAvailabilityDto(availabilityExternal.Facility, list);
    }

    private List<Slot> GetDaySlots(DateTime day, DayExternal? dayExternal, int slotDurationInMinutes)
    {
        if (dayExternal?.WorkPeriod is null)
        {
            return [];
        }
        
        var slotList = new List<Slot>();
        var morningSlots = GetSlots(
            new DateTime(day.Year, day.Month, day.Day, dayExternal.WorkPeriod.StartHour, 0, 0),
            new DateTime(day.Year, day.Month, day.Day, dayExternal.WorkPeriod.LunchStartHour, 0, 0),
            slotDurationInMinutes);
        var afternoonSlots = GetSlots(
            new DateTime(day.Year, day.Month, day.Day, dayExternal.WorkPeriod.LunchEndHour, 0, 0),
            new DateTime(day.Year, day.Month, day.Day, dayExternal.WorkPeriod.EndHour, 0, 0),
            slotDurationInMinutes);
        slotList.AddRange(morningSlots);
        slotList.AddRange(afternoonSlots);

        if (dayExternal.BusySlots is not null)
        {
            slotList = slotList.Where(x => !dayExternal.BusySlots.Any(b => b.Start == x.Start && b.End == x.End)).ToList();
        }

        return slotList;
    }

    private List<Slot> GetSlots(DateTime startDate, DateTime endDate, int slotDurationInMinutes)
    {
        var list = new List<Slot>();
        for (var start = startDate; start <= endDate.AddMinutes(-slotDurationInMinutes); start = start.AddMinutes(slotDurationInMinutes))
        {
            list.Add(new Slot(start, start.AddMinutes(slotDurationInMinutes)));
        }

        return list;
    }
}

public class GetTestFacilityWeeklyAvailabilityValidator : AbstractValidator<GetTestFacilityWeeklyAvailability>
{
    public GetTestFacilityWeeklyAvailabilityValidator()
    {
        RuleFor(x => x.MondayDate)
            .NotEmpty()
            .Length(8)
            .Must(x => x.All(char.IsDigit));
    }
}