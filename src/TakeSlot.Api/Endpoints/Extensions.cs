using TakeSlot.Api.Filters;

namespace TakeSlot.Api.Endpoints;

internal static class Extensions
{
    internal static void ExposeEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var global = endpointRouteBuilder
            .MapGroup(string.Empty);
        global.AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
        global.ExposeAvailabilityEndpoints();
    }
}