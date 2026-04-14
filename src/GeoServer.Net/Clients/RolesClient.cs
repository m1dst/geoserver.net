using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Security;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer security roles endpoints.
/// </summary>
public sealed class RolesClient : GeoServerClientBase
{
    internal RolesClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Executes the GetAllAsync operation.
    /// </summary>
    public Task<RolesListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<RolesListResponse>(HttpMethod.Get, "security/roles", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetAll operation.
    /// </summary>
    public RolesListResponse GetAll() => Send<RolesListResponse>(HttpMethod.Get, "security/roles");

    /// <summary>
    /// Executes the GetByUserAsync operation.
    /// </summary>
    public Task<RolesListResponse> GetByUserAsync(string userName, CancellationToken cancellationToken = default)
        => SendAsync<RolesListResponse>(HttpMethod.Get, $"security/roles/user/{Encode(userName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByUser operation.
    /// </summary>
    public RolesListResponse GetByUser(string userName)
        => Send<RolesListResponse>(HttpMethod.Get, $"security/roles/user/{Encode(userName)}");

    /// <summary>
    /// Executes the GetByGroupAsync operation.
    /// </summary>
    public Task<RolesListResponse> GetByGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync<RolesListResponse>(HttpMethod.Get, $"security/roles/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetByGroup operation.
    /// </summary>
    public RolesListResponse GetByGroup(string groupName)
        => Send<RolesListResponse>(HttpMethod.Get, $"security/roles/group/{Encode(groupName)}");

    /// <summary>
    /// Executes the AddRoleAsync operation.
    /// </summary>
    public Task AddRoleAsync(string roleName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the AddRole operation.
    /// </summary>
    public void AddRole(string roleName)
        => Send(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}");

    /// <summary>
    /// Executes the DeleteRoleAsync operation.
    /// </summary>
    public Task DeleteRoleAsync(string roleName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the DeleteRole operation.
    /// </summary>
    public void DeleteRole(string roleName)
        => Send(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}");

    /// <summary>
    /// Executes the AssociateRoleWithUserAsync operation.
    /// </summary>
    public Task AssociateRoleWithUserAsync(string roleName, string userName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the AssociateRoleWithUser operation.
    /// </summary>
    public void AssociateRoleWithUser(string roleName, string userName)
        => Send(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}");

    /// <summary>
    /// Executes the RemoveRoleFromUserAsync operation.
    /// </summary>
    public Task RemoveRoleFromUserAsync(string roleName, string userName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the RemoveRoleFromUser operation.
    /// </summary>
    public void RemoveRoleFromUser(string roleName, string userName)
        => Send(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}");

    /// <summary>
    /// Executes the AssociateRoleWithGroupAsync operation.
    /// </summary>
    public Task AssociateRoleWithGroupAsync(string roleName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the AssociateRoleWithGroup operation.
    /// </summary>
    public void AssociateRoleWithGroup(string roleName, string groupName)
        => Send(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/group/{Encode(groupName)}");

    /// <summary>
    /// Executes the RemoveRoleFromGroupAsync operation.
    /// </summary>
    public Task RemoveRoleFromGroupAsync(string roleName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the RemoveRoleFromGroup operation.
    /// </summary>
    public void RemoveRoleFromGroup(string roleName, string groupName)
        => Send(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}/group/{Encode(groupName)}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
