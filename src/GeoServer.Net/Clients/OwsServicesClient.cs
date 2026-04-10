using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Settings;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around OWS service settings endpoints (WMS/WFS/WCS/WMTS).
/// </summary>
public sealed class OwsServicesClient : GeoServerClientBase
{
    internal OwsServicesClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Gets global settings for a service type (wms/wfs/wcs/wmts).
    /// </summary>
    public Task<OwsServiceSettingsResponse> GetGlobalAsync(string serviceType, CancellationToken cancellationToken = default)
        => SendAsync<OwsServiceSettingsResponse>(HttpMethod.Get, $"services/{Normalize(serviceType)}/settings.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets global settings for a service type (synchronous).
    /// </summary>
    public OwsServiceSettingsResponse GetGlobal(string serviceType)
        => Send<OwsServiceSettingsResponse>(HttpMethod.Get, $"services/{Normalize(serviceType)}/settings.json");

    /// <summary>
    /// Updates global settings for a service type.
    /// </summary>
    public Task UpdateGlobalAsync(string serviceType, object settingsPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"services/{Normalize(serviceType)}/settings", settingsPayload, cancellationToken);

    /// <summary>
    /// Updates global settings for a service type (synchronous).
    /// </summary>
    public void UpdateGlobal(string serviceType, object settingsPayload)
        => Send(HttpMethod.Put, $"services/{Normalize(serviceType)}/settings", settingsPayload);

    /// <summary>
    /// Gets workspace-specific settings for a service type.
    /// </summary>
    public Task<OwsServiceSettingsResponse> GetWorkspaceAsync(string serviceType, string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<OwsServiceSettingsResponse>(HttpMethod.Get, $"services/{Normalize(serviceType)}/workspaces/{Encode(workspaceName)}/settings.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets workspace-specific settings for a service type (synchronous).
    /// </summary>
    public OwsServiceSettingsResponse GetWorkspace(string serviceType, string workspaceName)
        => Send<OwsServiceSettingsResponse>(HttpMethod.Get, $"services/{Normalize(serviceType)}/workspaces/{Encode(workspaceName)}/settings.json");

    /// <summary>
    /// Updates workspace-specific settings for a service type.
    /// </summary>
    public Task UpdateWorkspaceAsync(string serviceType, string workspaceName, object settingsPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"services/{Normalize(serviceType)}/workspaces/{Encode(workspaceName)}/settings", settingsPayload, cancellationToken);

    /// <summary>
    /// Updates workspace-specific settings for a service type (synchronous).
    /// </summary>
    public void UpdateWorkspace(string serviceType, string workspaceName, object settingsPayload)
        => Send(HttpMethod.Put, $"services/{Normalize(serviceType)}/workspaces/{Encode(workspaceName)}/settings", settingsPayload);

    /// <summary>
    /// Deletes workspace-specific settings for a service type.
    /// </summary>
    public Task DeleteWorkspaceAsync(string serviceType, string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"services/{Normalize(serviceType)}/workspaces/{Encode(workspaceName)}/settings", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes workspace-specific settings for a service type (synchronous).
    /// </summary>
    public void DeleteWorkspace(string serviceType, string workspaceName)
        => Send(HttpMethod.Delete, $"services/{Normalize(serviceType)}/workspaces/{Encode(workspaceName)}/settings");

    private static string Normalize(string serviceType)
    {
        if (string.IsNullOrWhiteSpace(serviceType))
        {
            throw new ArgumentException("Service type is required.", nameof(serviceType));
        }

        var normalized = serviceType.Trim().ToLowerInvariant();
        return normalized switch
        {
            "wms" => normalized,
            "wfs" => normalized,
            "wcs" => normalized,
            "wmts" => normalized,
            _ => throw new ArgumentException("Supported service types are wms, wfs, wcs, wmts.", nameof(serviceType))
        };
    }

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
