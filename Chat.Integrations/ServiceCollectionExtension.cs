using Chat.Integrations.SmsAero;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Integrations;

public static class ServiceCollectionExtension
{
    public static void AddSmsAero(this IServiceCollection collection)
    {
        collection.AddScoped<SmsAeroClient>();
    }
}