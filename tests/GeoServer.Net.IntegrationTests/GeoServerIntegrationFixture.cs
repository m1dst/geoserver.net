using System;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net;

namespace GeoServer.Net.IntegrationTests;

/// <summary>
/// Represents the GeoServerIntegrationFixture type.
/// </summary>
public sealed class GeoServerIntegrationFixture : IAsyncLifetime
{
    /// <summary>
    /// Gets or sets the BaseUri value.
    /// </summary>
    public Uri BaseUri { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the Username value.
    /// </summary>
    public string Username { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the Password value.
    /// </summary>
    public string Password { get; private set; } = default!;

    /// <summary>
    /// Executes the InitializeAsync operation.
    /// </summary>
    public async Task InitializeAsync()
    {
        var baseUrl = Environment.GetEnvironmentVariable("GEOSERVER_BASE_URL") ?? "http://localhost:8080/geoserver/rest/";
        BaseUri = new Uri(baseUrl);
        Username = Environment.GetEnvironmentVariable("GEOSERVER_USERNAME") ?? "admin";
        Password = Environment.GetEnvironmentVariable("GEOSERVER_PASSWORD") ?? "geoserver";
        await WaitForServerAsync(BaseUri).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the DisposeAsync operation.
    /// </summary>
    public Task DisposeAsync() => Task.CompletedTask;

    /// <summary>
    /// Executes the CreateClient operation.
    /// </summary>
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
