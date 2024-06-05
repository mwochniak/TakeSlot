using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TakeSlot.Application.Clients;
using TakeSlot.Application.Options;
using TakeSlot.Application.Services;
using TakeSlot.Infrastructure.Clients;
using TakeSlot.Infrastructure.Services;

namespace TakeSlot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSlotService(configuration)
            .AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAvailabilityOfFacilityService, AvailabilityOfFacilityService>();
        services.AddScoped<IHttpRequestSender, HttpRequestSender>();

        return services;
    }
    
    private static IServiceCollection AddSlotService(this IServiceCollection services, IConfiguration configuration)
    {
        var slotServiceApiClientOptions = configuration.GetOptions<SlotServiceApiClientOptions>(
            SlotServiceApiClientOptions.SlotServiceApiClient
        );

        services.AddSingleton(slotServiceApiClientOptions);

        services.AddHttpClient<ISlotServiceApiClient, SlotServiceApiClient>(client =>
            {
                client.BaseAddress = slotServiceApiClientOptions.BaseUrl;
                client.Timeout = slotServiceApiClientOptions.Timeout;
            }
        );
        
        services.AddSingleton<ISlotServiceAuthenticator, SlotServiceBasicAuthenticator>();

        return services;
    }

    private static T GetOptions<T>(this IConfiguration configuration, string sectionName)
        where T : new() => configuration.GetSection(sectionName).GetOptions<T>();

    private static T GetOptions<T>(this IConfiguration section)
        where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }
}