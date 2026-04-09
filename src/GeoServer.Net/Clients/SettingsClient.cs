using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Settings;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer settings REST endpoints.
/// </summary>
public sealed class SettingsClient : GeoServerClientBase
{
    internal SettingsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Gets global settings.
    /// </summary>
    public Task<GlobalSettingsResponse> GetGlobalAsync(CancellationToken cancellationToken = default)
        => SendAsync<GlobalSettingsResponse>(HttpMethod.Get, "settings.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets global settings (synchronous).
    /// </summary>
    public GlobalSettingsResponse GetGlobal()
        => Send<GlobalSettingsResponse>(HttpMethod.Get, "settings.json");

    /// <summary>
    /// Updates global settings.
    /// </summary>
    public Task UpdateGlobalAsync(object globalPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "settings", globalPayload, cancellationToken);

    /// <summary>
    /// Updates global settings (synchronous).
    /// </summary>
    public void UpdateGlobal(object globalPayload)
        => Send(HttpMethod.Put, "settings", globalPayload);

    /// <summary>
    /// Gets global contact settings.
    /// </summary>
    public Task<ContactSettingsResponse> GetContactAsync(CancellationToken cancellationToken = default)
        => SendAsync<ContactSettingsResponse>(HttpMethod.Get, "settings/contact.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets global contact settings (synchronous).
    /// </summary>
    public ContactSettingsResponse GetContact()
        => Send<ContactSettingsResponse>(HttpMethod.Get, "settings/contact.json");

    /// <summary>
    /// Updates global contact settings.
    /// </summary>
    public Task UpdateContactAsync(object contactPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, "settings/contact", contactPayload, cancellationToken);

    /// <summary>
    /// Updates global contact settings (synchronous).
    /// </summary>
    public void UpdateContact(object contactPayload)
        => Send(HttpMethod.Put, "settings/contact", contactPayload);

    /// <summary>
    /// Gets workspace-specific settings.
    /// </summary>
    public Task<WorkspaceSettingsResponse> GetWorkspaceAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<WorkspaceSettingsResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/settings.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets workspace-specific settings (synchronous).
    /// </summary>
    public WorkspaceSettingsResponse GetWorkspace(string workspaceName)
        => Send<WorkspaceSettingsResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}/settings.json");

    /// <summary>
    /// Creates workspace-specific settings.
    /// </summary>
    public Task CreateWorkspaceAsync(string workspaceName, object settingsPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/settings", settingsPayload, cancellationToken);

    /// <summary>
    /// Creates workspace-specific settings (synchronous).
    /// </summary>
    public void CreateWorkspace(string workspaceName, object settingsPayload)
        => Send(HttpMethod.Post, $"workspaces/{Encode(workspaceName)}/settings", settingsPayload);

    /// <summary>
    /// Updates workspace-specific settings.
    /// </summary>
    public Task UpdateWorkspaceAsync(string workspaceName, object settingsPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/settings", settingsPayload, cancellationToken);

    /// <summary>
    /// Updates workspace-specific settings (synchronous).
    /// </summary>
    public void UpdateWorkspace(string workspaceName, object settingsPayload)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}/settings", settingsPayload);

    /// <summary>
    /// Deletes workspace-specific settings.
    /// </summary>
    public Task DeleteWorkspaceAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/settings", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes workspace-specific settings (synchronous).
    /// </summary>
    public void DeleteWorkspace(string workspaceName)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}/settings");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
