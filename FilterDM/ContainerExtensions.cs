using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace FilterDM;

public static class ContainerExtensions
{
    public static IServiceCollection BindService<TService>(this IServiceCollection services) where TService : class
    {
        services.AddSingleton<TService>();

        var interfaces = typeof(TService).GetInterfaces();
        foreach (var iface in interfaces)
        {
            services.AddSingleton(iface, provider => provider.GetRequiredService<TService>());
        }

        return services;
    }
}