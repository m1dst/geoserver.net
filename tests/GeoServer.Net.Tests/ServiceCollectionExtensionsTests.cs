using System;
using geoserver.net;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class ServiceCollectionExtensionsTests
{
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
        var client = scope.ServiceProvider.GetRequiredService<geoserver.net.GeoServerClient>();
        Assert.NotNull(client);
    }

    [Fact]
    public void AddGeoServerClient_WithPrebuiltOptions_ResolvesClient()
    {
        var services = new ServiceCollection();
        services.AddGeoServerClient(new geoserver.net.GeoServerClientOptions
        {
            BaseUri = new Uri("http://localhost:8080/geoserver/rest/"),
            Username = "admin",
            Password = "geoserver",
            Timeout = TimeSpan.FromSeconds(60)
        });

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<geoserver.net.GeoServerClient>();
        Assert.NotNull(client);
    }

    [Fact]
    public void AddGeoServerClient_ThrowsWhenBaseUriMissing()
    {
        var services = new ServiceCollection();
        services.AddGeoServerClient(_ => { });

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var ex = Assert.Throws<InvalidOperationException>(
            () => scope.ServiceProvider.GetRequiredService<geoserver.net.GeoServerClient>());
        Assert.Contains("BaseUri", ex.Message);
    }
}
