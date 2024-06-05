using FluentValidation;
using LanguageExt.Common;
using MediatR;
using TakeSlot.Application.Results;
using TakeSlot.Application.Services;

namespace TakeSlot.Application.Commands;

public record TakeTestFacilitySlot(Guid FacilityId, DateTime Start, DateTime End, string Comments, Patient Patient) : IRequest<Result<BaseResult>>;
public record Patient(string Name, string SecondName, string Email, string Phone);

internal sealed class TakeTestFacilitySlotHandler(IAvailabilityOfFacilityService service) : IRequestHandler<TakeTestFacilitySlot, Result<BaseResult>> 
{
    public async Task<Result<BaseResult>> Handle(TakeTestFacilitySlot request, CancellationToken cancellationToken)
    {
        var result = await service.TakeTestFacilitySlotAsync(request, cancellationToken);
        return result.Match(succ => succ, err => new Result<BaseResult>(err));
    }
}

public class TakeTestSpecialistSlotValidator : AbstractValidator<TakeTestFacilitySlot>
{
    public TakeTestSpecialistSlotValidator()
    {
        RuleFor(x => x.FacilityId).NotEmpty();
        RuleFor(x => x.Start).NotEmpty();
        RuleFor(x => x.Start).LessThan(x => x.End);
        RuleFor(x => x.End).NotEmpty();
        RuleFor(x => x.Patient.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Patient.Name).NotEmpty();
        RuleFor(x => x.Patient.Phone).NotEmpty();
    }
}