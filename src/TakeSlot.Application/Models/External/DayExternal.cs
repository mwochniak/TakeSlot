namespace TakeSlot.Application.Models.External;

public record DayExternal(WorkPeriod? WorkPeriod, IEnumerable<Slot>? BusySlots);