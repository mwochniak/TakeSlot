namespace TakeSlot.Application.Exceptions;

public class TakeSlotValidationException(List<string> errors) : TakeSlotException(string.Empty)
{
    public List<string> Errors { get; } = errors;
}