namespace TakeSlot.Application.Models;

public record FacilityAvailabilityDto(Facility Facility, IEnumerable<Slot> FreeSlots);