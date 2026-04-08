using System;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net;

namespace GeoServer.Net.IntegrationTests;

public sealed class GeoServerIntegrationFixture : IAsyncLifetime
{
    public Uri BaseUri { get; private set; } = default!;

    public string Username { get; private set; } = default!;

    public string Password { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        var baseUrl = Environment.GetEnvironmentVariable("GEOSERVER_BASE_URL") ?? "http://localhost:8080/geoserver/rest/";
        BaseUri = new Uri(baseUrl);
        Username = Environment.GetEnvironmentVariable("GEOSERVER_USERNAME") ?? "admin";
        Password = Environment.GetEnvironmentVariable("GEOSERVER_PASSWORD") ?? "geoserver";
        await WaitForServerAsync(BaseUri).ConfigureAwait(false);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    public GeoServerClient CreateClient()
        => new(new GeoServerClientOptions
        {
            BaseUri = BaseUri,
            Username = Username,
            Password = Password,
            Timeout = TimeSpan.FromSeconds(120)
        });

    private static async Task WaitForServerAsync(Uri baseUri)
    {
        using var httpClient = new HttpClient { BaseAddress = baseUri, Timeout = TimeSpan.FromSeconds(10) };
        for (var i = 0; i < 60; i++)
        {
            try
            {
                using var response = await httpClient.GetAsync("about/status.json").ConfigureAwait(false);
                if ((int)response.StatusCode < 500)
                {
                    return;
                }
            }
            catch
            {
            }

            await Task.Delay(2000).ConfigureAwait(false);
        }

        throw new TimeoutException("Timed out waiting for GeoServer REST endpoint.");
    }
}
