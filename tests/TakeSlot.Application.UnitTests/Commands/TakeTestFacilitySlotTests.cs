using LanguageExt.Common;
using Moq;
using TakeSlot.Application.Commands;
using TakeSlot.Application.Exceptions;
using TakeSlot.Application.Results;
using TakeSlot.Application.Services;
using Xunit;

namespace TakeSlot.Application.UnitTests.Commands;

public sealed class TakeTestFacilitySlotTests
{
    private readonly TakeTestFacilitySlotHandler _handler; 
    private readonly Mock<IAvailabilityOfFacilityService> _availabilityServiceMock = new();
    
    public TakeTestFacilitySlotTests()
    {
        _handler = new(_availabilityServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_TakeSlot_WhenServiceIsAvailable()
    {
        var request = new TakeTestFacilitySlot(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddMinutes(30),
            "Comment", new Patient("Jan", "Kowalski", "jan.kowalski@email.com", "100-200-300"));

        _availabilityServiceMock.Setup(s => s.TakeTestFacilitySlotAsync(It.IsAny<TakeTestFacilitySlot>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Result<BaseResult>(new BaseResult()));
        
        var result = await _handler.Handle(request, new CancellationToken());
        
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFalseResult_WhenServiceRequestFailed()
    {
        var request = new TakeTestFacilitySlot(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddMinutes(30),
            "Comment", new Patient("Jan", "Kowalski", "jan.kowalski@email.com", "100-200-300"));

        _availabilityServiceMock.Setup(s => s.TakeTestFacilitySlotAsync(It.IsAny<TakeTestFacilitySlot>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Result<BaseResult>(new TakeSlotException("500 error")));
        
        var result = await _handler.Handle(request, new CancellationToken());
        
        Assert.True(result.IsFaulted);
    }
}