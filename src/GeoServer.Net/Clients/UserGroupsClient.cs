using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Security;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer security user/group endpoints.
/// </summary>
public sealed class UserGroupsClient : GeoServerClientBase
{
    internal UserGroupsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<UsersListResponse> GetUsersAsync(CancellationToken cancellationToken = default)
        => SendAsync<UsersListResponse>(HttpMethod.Get, "security/usergroup/users", cancellationToken: cancellationToken);

    public UsersListResponse GetUsers()
        => Send<UsersListResponse>(HttpMethod.Get, "security/usergroup/users");

    public Task CreateUserAsync(GeoServerUserDto user, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "security/usergroup/users", user, cancellationToken);

    public void CreateUser(GeoServerUserDto user)
        => Send(HttpMethod.Post, "security/usergroup/users", user);

    public Task UpdateUserAsync(string userName, GeoServerUserDto user, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}", user, cancellationToken);

    public void UpdateUser(string userName, GeoServerUserDto user)
        => Send(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}", user);

    public Task DeleteUserAsync(string userName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/usergroup/user/{Encode(userName)}", cancellationToken: cancellationToken);

    public void DeleteUser(string userName)
        => Send(HttpMethod.Delete, $"security/usergroup/user/{Encode(userName)}");

    public Task<GroupsListResponse> GetGroupsAsync(CancellationToken cancellationToken = default)
        => SendAsync<GroupsListResponse>(HttpMethod.Get, "security/usergroup/groups", cancellationToken: cancellationToken);

    public GroupsListResponse GetGroups()
        => Send<GroupsListResponse>(HttpMethod.Get, "security/usergroup/groups");

    public Task AddGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/usergroup/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    public void AddGroup(string groupName)
        => Send(HttpMethod.Post, $"security/usergroup/group/{Encode(groupName)}");

    public Task DeleteGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/usergroup/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    public void DeleteGroup(string groupName)
        => Send(HttpMethod.Delete, $"security/usergroup/group/{Encode(groupName)}");

    public Task<UsersListResponse> GetUsersForGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync<UsersListResponse>(HttpMethod.Get, $"security/usergroup/group/{Encode(groupName)}/users", cancellationToken: cancellationToken);

    public UsersListResponse GetUsersForGroup(string groupName)
        => Send<UsersListResponse>(HttpMethod.Get, $"security/usergroup/group/{Encode(groupName)}/users");

    public Task<GroupsListResponse> GetGroupsForUserAsync(string userName, CancellationToken cancellationToken = default)
        => SendAsync<GroupsListResponse>(HttpMethod.Get, $"security/usergroup/user/{Encode(userName)}/groups", cancellationToken: cancellationToken);

    public GroupsListResponse GetGroupsForUser(string userName)
        => Send<GroupsListResponse>(HttpMethod.Get, $"security/usergroup/user/{Encode(userName)}/groups");

    public Task AssociateUserWithGroupAsync(string userName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    public void AssociateUserWithGroup(string userName, string groupName)
        => Send(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}/group/{Encode(groupName)}");

    public Task RemoveUserFromGroupAsync(string userName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/usergroup/user/{Encode(userName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    public void RemoveUserFromGroup(string userName, string groupName)
        => Send(HttpMethod.Delete, $"security/usergroup/user/{Encode(userName)}/group/{Encode(groupName)}");

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
