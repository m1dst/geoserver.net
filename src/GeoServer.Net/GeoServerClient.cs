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
        Coverages = new CoveragesClient(_httpClient);
        StructuredCoverages = new StructuredCoveragesClient(_httpClient);
        FeatureTypes = new FeatureTypesClient(_httpClient);
        Namespaces = new NamespacesClient(_httpClient);
        LayerGroups = new LayerGroupsClient(_httpClient);
        Layers = new LayersClient(_httpClient);
        Styles = new StylesClient(_httpClient);
        Settings = new SettingsClient(_httpClient);
        OwsServices = new OwsServicesClient(_httpClient);
        Roles = new RolesClient(_httpClient);
        UserGroups = new UserGroupsClient(_httpClient);
        AuthProviders = new AuthProvidersClient(_httpClient);
        AuthFilters = new AuthFiltersClient(_httpClient);
        WmsStores = new WmsStoresClient(_httpClient);
        WmsLayers = new WmsLayersClient(_httpClient);
        WmtsStores = new WmtsStoresClient(_httpClient);
        WmtsLayers = new WmtsLayersClient(_httpClient);
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
        Coverages = new CoveragesClient(_httpClient);
        StructuredCoverages = new StructuredCoveragesClient(_httpClient);
        FeatureTypes = new FeatureTypesClient(_httpClient);
        Namespaces = new NamespacesClient(_httpClient);
        LayerGroups = new LayerGroupsClient(_httpClient);
        Layers = new LayersClient(_httpClient);
        Styles = new StylesClient(_httpClient);
        Settings = new SettingsClient(_httpClient);
        OwsServices = new OwsServicesClient(_httpClient);
        Roles = new RolesClient(_httpClient);
        UserGroups = new UserGroupsClient(_httpClient);
        AuthProviders = new AuthProvidersClient(_httpClient);
        AuthFilters = new AuthFiltersClient(_httpClient);
        WmsStores = new WmsStoresClient(_httpClient);
        WmsLayers = new WmsLayersClient(_httpClient);
        WmtsStores = new WmtsStoresClient(_httpClient);
        WmtsLayers = new WmtsLayersClient(_httpClient);
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
    /// Coverage endpoints.
    /// </summary>
    public CoveragesClient Coverages { get; }

    /// <summary>
    /// Structured coverage endpoints.
    /// </summary>
    public StructuredCoveragesClient StructuredCoverages { get; }

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

    /// <summary>
    /// Global and workspace settings endpoints.
    /// </summary>
    public SettingsClient Settings { get; }

    /// <summary>
    /// OWS service settings endpoints.
    /// </summary>
    public OwsServicesClient OwsServices { get; }

    /// <summary>
    /// Security roles endpoints.
    /// </summary>
    public RolesClient Roles { get; }

    /// <summary>
    /// Security users and groups endpoints.
    /// </summary>
    public UserGroupsClient UserGroups { get; }

    /// <summary>
    /// Security authentication providers endpoints.
    /// </summary>
    public AuthProvidersClient AuthProviders { get; }

    /// <summary>
    /// Security authentication filters endpoints.
    /// </summary>
    public AuthFiltersClient AuthFilters { get; }

    /// <summary>
    /// WMS stores endpoints.
    /// </summary>
    public WmsStoresClient WmsStores { get; }

    /// <summary>
    /// WMS layers endpoints.
    /// </summary>
    public WmsLayersClient WmsLayers { get; }

    /// <summary>
    /// WMTS stores endpoints.
    /// </summary>
    public WmtsStoresClient WmtsStores { get; }

    /// <summary>
    /// WMTS layers endpoints.
    /// </summary>
    public WmtsLayersClient WmtsLayers { get; }

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
