using LanguageExt.Common;

namespace TakeSlot.Api.Results;

public interface IResultMapper
{
    IResult Map<T>(Result<T> result);
}