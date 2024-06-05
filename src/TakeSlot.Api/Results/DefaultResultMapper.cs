using LanguageExt.Common;
using TakeSlot.Application.Exceptions;

namespace TakeSlot.Api.Results;

public class DefaultResultMapper : IResultMapper
{
    public IResult Map<T>(Result<T> result)
    {
        return result.Match<IResult>(Microsoft.AspNetCore.Http.Results.Ok, ex => ex switch
        {
            TakeSlotValidationException takeSlotException => Microsoft.AspNetCore.Http.Results.BadRequest(string.Join(", ", takeSlotException.Errors)),
            { } exception => Microsoft.AspNetCore.Http.Results.BadRequest(exception.Message),
            _ => Microsoft.AspNetCore.Http.Results.BadRequest(ex.Message)
        });
    }
}