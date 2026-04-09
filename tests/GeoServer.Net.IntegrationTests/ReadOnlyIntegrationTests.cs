using System;
using System.Threading.Tasks;
using geoserver.net;
using Xunit;

namespace GeoServer.Net.IntegrationTests;

public sealed class ReadOnlyIntegrationTests : IClassFixture<GeoServerIntegrationFixture>
{
    private readonly GeoServerIntegrationFixture _fixture;
    private bool _gwcUnavailable;
    private bool _importerUnavailable;

    public ReadOnlyIntegrationTests(GeoServerIntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AboutEndpoints_ReturnMetadata()
    {
        using var client = _fixture.CreateClient();

        var version = await client.About.GetVersionAsync();
        Assert.NotNull(version.About);

        var status = await client.About.GetStatusAsync();
        Assert.NotNull(status.About);
    }

    [Fact]
    public async Task GeoWebCacheCoreReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_gwcUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();

        try
        {
            var index = await client.GeoWebCache.GetIndexRawAsync();
            Assert.False(string.IsNullOrWhiteSpace(index));

            var global = await client.GeoWebCache.GetGlobalAsync();
            Assert.NotNull(global.Global);
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404)
        {
            _gwcUnavailable = true;
            return;
        }
    }

    [Fact]
    public async Task ImporterReadOnly_ReturnsDataOrSkipsIfUnavailable()
    {
        if (_importerUnavailable)
        {
            return;
        }

        using var client = _fixture.CreateClient();

        try
        {
            var imports = await client.Importer.GetAllAsync();
            Assert.NotNull(imports.Payload);
        }
        catch (GeoServerApiException ex) when ((int)ex.StatusCode == 404)
        {
            _importerUnavailable = true;
            return;
        }
    }
}
