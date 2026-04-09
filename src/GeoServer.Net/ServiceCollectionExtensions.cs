using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace geoserver.net;

/// <summary>
/// Dependency injection registration extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="GeoServerClient"/> for DI and configures options.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configureOptions">Options callback.</param>
    public static IServiceCollection AddGeoServerClient(
        this IServiceCollection services,
        Action<GeoServerClientOptions> configureOptions)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        services.Configure(configureOptions);
        services.AddHttpClient<GeoServerClient>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<GeoServerClientOptions>>().Value;
            try
            {
                GeoServerHttpClient.ApplyOptions(httpClient, options);
            }
            catch (ArgumentException ex) when (ex.ParamName == nameof(options))
            {
                throw new InvalidOperationException("GeoServerClientOptions.BaseUri must be configured.", ex);
            }
        });

        return services;
    }

    /// <summary>
    /// Registers <see cref="GeoServerClient"/> for DI using pre-built options.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="options">GeoServer options.</param>
    public static IServiceCollection AddGeoServerClient(
        this IServiceCollection services,
        GeoServerClientOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return services.AddGeoServerClient(config =>
        {
            config.BaseUri = options.BaseUri;
            config.Username = options.Username;
            config.Password = options.Password;
            config.Timeout = options.Timeout;
        });
    }

}
