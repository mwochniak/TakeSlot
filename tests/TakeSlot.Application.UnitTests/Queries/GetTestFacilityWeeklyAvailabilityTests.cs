using LanguageExt.Common;
using Moq;
using TakeSlot.Application.Exceptions;
using TakeSlot.Application.Models;
using TakeSlot.Application.Models.External;
using TakeSlot.Application.Queries;
using TakeSlot.Application.Services;
using Xunit;

namespace TakeSlot.Application.UnitTests.Queries;

public class GetTestFacilityWeeklyAvailabilityTests
{
    private readonly GetTestFacilityAvailabilityHandler _handler; 
    private readonly Mock<IAvailabilityOfFacilityService> _availabilityServiceMock = new();
    
    public GetTestFacilityWeeklyAvailabilityTests()
    {
        _handler = new(_availabilityServiceMock.Object);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnValidationError_WhenDayIsNotMonday()
    {
        var request = new GetTestFacilityWeeklyAvailability("20240604");

        _availabilityServiceMock.Setup(s => s.GetTestFacilityWeeklyAvailabilityAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Result<FacilityAvailabilityExternalDto>(GetExternalDto()));
        
        var result = await _handler.Handle(request, new CancellationToken());
        
        Assert.True(result.IsFaulted);
        var exception = new Exception();
        result.IfFail(ex => exception = ex);
        Assert.Equal(typeof(TakeSlotValidationException), exception.GetType());
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFilteredCollection_WhenBusySlotsOccurs()
    {
        var request = new GetTestFacilityWeeklyAvailability("20240603");

        _availabilityServiceMock.Setup(s => s.GetTestFacilityWeeklyAvailabilityAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Result<FacilityAvailabilityExternalDto>(GetExternalDto()));
        
        var result = await _handler.Handle(request, new CancellationToken());
        
        Assert.True(result.IsSuccess);
        FacilityAvailabilityDto? dto = null;
        result.IfSucc(r => dto = r);
        Assert.Contains(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 8, 10,0) && fs.End == (new DateTime(2024, 06, 04, 8, 20,0)));
        Assert.Contains(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 11, 50,0) && fs.End == (new DateTime(2024, 06, 04, 12, 00,0)));
        Assert.Contains(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 13, 00,0) && fs.End == (new DateTime(2024, 06, 04, 13, 10,0)));
        Assert.DoesNotContain(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 8, 0,0) && fs.End == (new DateTime(2024, 06, 04, 8, 10,0)));
        Assert.DoesNotContain(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 8, 20,0) && fs.End == (new DateTime(2024, 06, 04, 8, 30,0)));
        Assert.DoesNotContain(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 8, 40,0) && fs.End == (new DateTime(2024, 06, 04, 8, 50,0)));
        Assert.DoesNotContain(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 16, 0,0) && fs.End == (new DateTime(2024, 06, 04, 16, 10,0)));
        Assert.DoesNotContain(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 12, 0,0) && fs.End == (new DateTime(2024, 06, 04, 12, 10,0)));
        Assert.DoesNotContain(dto!.FreeSlots, fs => fs.Start == new DateTime(2024, 06, 04, 12, 50,0) && fs.End == (new DateTime(2024, 06, 04, 13, 00,0)));
    }

    private static FacilityAvailabilityExternalDto GetExternalDto()
    {
        return new FacilityAvailabilityExternalDto(new Facility(Guid.NewGuid(), "Name", "Address"), 10,
            new DayExternal(new WorkPeriod(7, 15, 11, 12), null),
            new DayExternal(new WorkPeriod(8, 16, 12, 13), new List<Slot>()
            {
                new Slot(new DateTime(2024, 06, 04, 8, 0,0), (new DateTime(2024, 06, 04, 8, 10,0))),
                new Slot(new DateTime(2024, 06, 04, 8, 20,0), (new DateTime(2024, 06, 04, 8, 30,0))),
                new Slot(new DateTime(2024, 06, 04, 8, 40,0), (new DateTime(2024, 06, 04, 8, 50,0))),
            }),
            new DayExternal(null, null),
            new DayExternal(null, null),
            new DayExternal(null, null),
            null,
            new DayExternal(null, null));
    }
}