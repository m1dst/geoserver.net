using System;
using System.Net.Http;
using geoserver.net.Clients;

namespace geoserver.net;

/// <summary>
/// Root client for GeoServer REST wrapper.
/// </summary>
public sealed class GeoServerClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private bool _disposed;

    /// <summary>
    /// Creates a new client from options.
    /// </summary>
    public GeoServerClient(GeoServerClientOptions options)
    {
        _httpClient = GeoServerHttpClient.Create(options);
        _ownsHttpClient = true;
        Workspaces = new WorkspacesClient(_httpClient);
        DataStores = new DataStoresClient(_httpClient);
        CoverageStores = new CoverageStoresClient(_httpClient);
        FeatureTypes = new FeatureTypesClient(_httpClient);
        Namespaces = new NamespacesClient(_httpClient);
        LayerGroups = new LayerGroupsClient(_httpClient);
        Layers = new LayersClient(_httpClient);
        Styles = new StylesClient(_httpClient);
    }

    /// <summary>
    /// Creates a new client using an existing <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="httpClient">Configured HTTP client.</param>
    /// <param name="disposeHttpClient">Whether dispose should also dispose the provided client.</param>
    public GeoServerClient(HttpClient httpClient, bool disposeHttpClient = false)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ownsHttpClient = disposeHttpClient;
        Workspaces = new WorkspacesClient(_httpClient);
        DataStores = new DataStoresClient(_httpClient);
        CoverageStores = new CoverageStoresClient(_httpClient);
        FeatureTypes = new FeatureTypesClient(_httpClient);
        Namespaces = new NamespacesClient(_httpClient);
        LayerGroups = new LayerGroupsClient(_httpClient);
        Layers = new LayersClient(_httpClient);
        Styles = new StylesClient(_httpClient);
    }

    /// <summary>
    /// Workspace endpoints.
    /// </summary>
    public WorkspacesClient Workspaces { get; }

    /// <summary>
    /// Data store endpoints.
    /// </summary>
    public DataStoresClient DataStores { get; }

    /// <summary>
    /// Coverage store endpoints.
    /// </summary>
    public CoverageStoresClient CoverageStores { get; }

    /// <summary>
    /// Feature type endpoints.
    /// </summary>
    public FeatureTypesClient FeatureTypes { get; }

    /// <summary>
    /// Namespace endpoints.
    /// </summary>
    public NamespacesClient Namespaces { get; }

    /// <summary>
    /// Layer group endpoints.
    /// </summary>
    public LayerGroupsClient LayerGroups { get; }

    /// <summary>
    /// Layer endpoints.
    /// </summary>
    public LayersClient Layers { get; }

    /// <summary>
    /// Style endpoints.
    /// </summary>
    public StylesClient Styles { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }

        _disposed = true;
    }
}
