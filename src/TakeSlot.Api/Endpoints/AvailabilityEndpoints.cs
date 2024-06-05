using System.Net;
using MediatR;
using TakeSlot.Api.Filters;
using TakeSlot.Api.Results;
using TakeSlot.Application.Commands;
using TakeSlot.Application.Queries;

namespace TakeSlot.Api.Endpoints;

internal static class AvailabilityEndpoints
{
    private const string Availability = nameof(Availability);

    internal static void ExposeAvailabilityEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("availability/weekly/facilities/test", async (IMediator mediator, string mondayDate, IResultMapper resultMapper, CancellationToken cancellationToken) =>
            {
                var query = new GetTestFacilityWeeklyAvailability(mondayDate);
                var validator = new GetTestFacilityWeeklyAvailabilityValidator();
                var validationResult = await validator.ValidateAsync(query, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return Microsoft.AspNetCore.Http.Results.ValidationProblem(validationResult.ToDictionary(),
                        statusCode: (int)HttpStatusCode.UnprocessableEntity);
                }
                
                var queryResult = await mediator.Send(query, cancellationToken);
                return resultMapper.Map(queryResult);
            })
            .WithTags(Availability)
            .WithDescription("Provide monday date format as yyyyMMdd ex: 20240603")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK);
        
        
        endpointRouteBuilder.MapPut("availability/slot", async (IMediator mediator, [Validate] TakeTestFacilitySlot command, IResultMapper resultMapper, CancellationToken cancellationToken) =>
            {
                var commandResult = await mediator.Send(command, cancellationToken);
                return resultMapper.Map(commandResult);
            })
            .WithTags(Availability)
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK);
    }
}