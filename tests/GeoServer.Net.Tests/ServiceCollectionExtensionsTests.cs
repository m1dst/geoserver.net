using System;
using GeoServer;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the ServiceCollectionExtensionsTests type.
/// </summary>
public sealed class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Executes the AddGeoServerClient_WithConfigureOptions_ResolvesClient operation.
    /// </summary>
    [Fact]
    public void AddGeoServerClient_WithConfigureOptions_ResolvesClient()
    {
        var services = new ServiceCollection();
        services.AddGeoServerClient(options =>
        {
            options.BaseUri = new Uri("http://localhost:8080/geoserver/rest/");
            options.Username = "admin";
            options.Password = "geoserver";
            options.Timeout = TimeSpan.FromSeconds(30);
        });

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<GeoServer.GeoServerClient>();
        Assert.NotNull(client);
    }

    /// <summary>
    /// Executes the AddGeoServerClient_WithPrebuiltOptions_ResolvesClient operation.
    /// </summary>
    [Fact]
    public void AddGeoServerClient_WithPrebuiltOptions_ResolvesClient()
    {
        var services = new ServiceCollection();
        services.AddGeoServerClient(new GeoServer.GeoServerClientOptions
        {
            BaseUri = new Uri("http://localhost:8080/geoserver/rest/"),
            Username = "admin",
            Password = "geoserver",
            Timeout = TimeSpan.FromSeconds(60)
        });

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<GeoServer.GeoServerClient>();
        Assert.NotNull(client);
    }

    /// <summary>
    /// Executes the AddGeoServerClient_ThrowsWhenBaseUriMissing operation.
    /// </summary>
    [Fact]
    public void AddGeoServerClient_ThrowsWhenBaseUriMissing()
    {
        var services = new ServiceCollection();
        services.AddGeoServerClient(_ => { });

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var ex = Assert.Throws<InvalidOperationException>(
            () => scope.ServiceProvider.GetRequiredService<GeoServer.GeoServerClient>());
        Assert.Contains("BaseUri", ex.Message);
    }
}
