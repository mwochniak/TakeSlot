namespace TakeSlot.Application.Models.External;

public record FacilityAvailabilityExternalDto(Facility Facility, int SlotDurationMinutes, DayExternal? Monday, DayExternal? Tuesday, DayExternal? Wednesday, DayExternal? Thursday, DayExternal? Friday, DayExternal? Saturday, DayExternal? Sunday);