using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Workspaces;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer workspaces REST endpoints.
/// </summary>
public sealed class WorkspacesClient : GeoServerClientBase
{
    internal WorkspacesClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Gets all workspaces.
    /// </summary>
    public Task<WorkspaceListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<WorkspaceListResponse>(HttpMethod.Get, "workspaces.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets all workspaces (synchronous).
    /// </summary>
    public WorkspaceListResponse GetAll() => Send<WorkspaceListResponse>(HttpMethod.Get, "workspaces.json");

    /// <summary>
    /// Gets one workspace by name.
    /// </summary>
    public Task<WorkspaceResponse> GetByNameAsync(string workspaceName, CancellationToken cancellationToken = default)
        => SendAsync<WorkspaceResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}.json", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one workspace by name (synchronous).
    /// </summary>
    public WorkspaceResponse GetByName(string workspaceName)
        => Send<WorkspaceResponse>(HttpMethod.Get, $"workspaces/{Encode(workspaceName)}.json");

    /// <summary>
    /// Creates a workspace.
    /// </summary>
    public Task CreateAsync(WorkspaceCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "workspaces", new { workspace = request }, cancellationToken);

    /// <summary>
    /// Creates a workspace (synchronous).
    /// </summary>
    public void Create(WorkspaceCreateRequest request)
        => Send(HttpMethod.Post, "workspaces", new { workspace = request });

    /// <summary>
    /// Updates a workspace.
    /// </summary>
    public Task UpdateAsync(string workspaceName, WorkspaceCreateRequest request, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}", new { workspace = request }, cancellationToken);

    /// <summary>
    /// Updates a workspace (synchronous).
    /// </summary>
    public void Update(string workspaceName, WorkspaceCreateRequest request)
        => Send(HttpMethod.Put, $"workspaces/{Encode(workspaceName)}", new { workspace = request });

    /// <summary>
    /// Deletes a workspace.
    /// </summary>
    public Task DeleteAsync(string workspaceName, bool recurse = false, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}?recurse={recurse.ToString().ToLowerInvariant()}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a workspace (synchronous).
    /// </summary>
    public void Delete(string workspaceName, bool recurse = false)
        => Send(HttpMethod.Delete, $"workspaces/{Encode(workspaceName)}?recurse={recurse.ToString().ToLowerInvariant()}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
