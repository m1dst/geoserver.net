using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Security;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer security roles endpoints.
/// </summary>
public sealed class RolesClient : GeoServerClientBase
{
    internal RolesClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<RolesListResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<RolesListResponse>(HttpMethod.Get, "security/roles", cancellationToken: cancellationToken);

    public RolesListResponse GetAll() => Send<RolesListResponse>(HttpMethod.Get, "security/roles");

    public Task<RolesListResponse> GetByUserAsync(string userName, CancellationToken cancellationToken = default)
        => SendAsync<RolesListResponse>(HttpMethod.Get, $"security/roles/user/{Encode(userName)}", cancellationToken: cancellationToken);

    public RolesListResponse GetByUser(string userName)
        => Send<RolesListResponse>(HttpMethod.Get, $"security/roles/user/{Encode(userName)}");

    public Task<RolesListResponse> GetByGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync<RolesListResponse>(HttpMethod.Get, $"security/roles/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    public RolesListResponse GetByGroup(string groupName)
        => Send<RolesListResponse>(HttpMethod.Get, $"security/roles/group/{Encode(groupName)}");

    public Task AddRoleAsync(string roleName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}", cancellationToken: cancellationToken);

    public void AddRole(string roleName)
        => Send(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}");

    public Task DeleteRoleAsync(string roleName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}", cancellationToken: cancellationToken);

    public void DeleteRole(string roleName)
        => Send(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}");

    public Task AssociateRoleWithUserAsync(string roleName, string userName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}", cancellationToken: cancellationToken);

    public void AssociateRoleWithUser(string roleName, string userName)
        => Send(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}");

    public Task RemoveRoleFromUserAsync(string roleName, string userName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}", cancellationToken: cancellationToken);

    public void RemoveRoleFromUser(string roleName, string userName)
        => Send(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}/user/{Encode(userName)}");

    public Task AssociateRoleWithGroupAsync(string roleName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    public void AssociateRoleWithGroup(string roleName, string groupName)
        => Send(HttpMethod.Post, $"security/roles/role/{Encode(roleName)}/group/{Encode(groupName)}");

    public Task RemoveRoleFromGroupAsync(string roleName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/roles/role/{Encode(roleName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

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
