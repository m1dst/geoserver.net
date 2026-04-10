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
    internal UserGroupsClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Executes the GetUsersAsync operation.
    /// </summary>
    public Task<UsersListResponse> GetUsersAsync(CancellationToken cancellationToken = default)
        => SendAsync<UsersListResponse>(HttpMethod.Get, "security/usergroup/users", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetUsers operation.
    /// </summary>
    public UsersListResponse GetUsers()
        => Send<UsersListResponse>(HttpMethod.Get, "security/usergroup/users");

    /// <summary>
    /// Executes the CreateUserAsync operation.
    /// </summary>
    public Task CreateUserAsync(GeoServerUserDto user, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "security/usergroup/users", user, cancellationToken);

    /// <summary>
    /// Executes the CreateUser operation.
    /// </summary>
    public void CreateUser(GeoServerUserDto user)
        => Send(HttpMethod.Post, "security/usergroup/users", user);

    /// <summary>
    /// Executes the UpdateUserAsync operation.
    /// </summary>
    public Task UpdateUserAsync(string userName, GeoServerUserDto user, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}", user, cancellationToken);

    /// <summary>
    /// Executes the UpdateUser operation.
    /// </summary>
    public void UpdateUser(string userName, GeoServerUserDto user)
        => Send(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}", user);

    /// <summary>
    /// Executes the DeleteUserAsync operation.
    /// </summary>
    public Task DeleteUserAsync(string userName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/usergroup/user/{Encode(userName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the DeleteUser operation.
    /// </summary>
    public void DeleteUser(string userName)
        => Send(HttpMethod.Delete, $"security/usergroup/user/{Encode(userName)}");

    /// <summary>
    /// Executes the GetGroupsAsync operation.
    /// </summary>
    public Task<GroupsListResponse> GetGroupsAsync(CancellationToken cancellationToken = default)
        => SendAsync<GroupsListResponse>(HttpMethod.Get, "security/usergroup/groups", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetGroups operation.
    /// </summary>
    public GroupsListResponse GetGroups()
        => Send<GroupsListResponse>(HttpMethod.Get, "security/usergroup/groups");

    /// <summary>
    /// Executes the AddGroupAsync operation.
    /// </summary>
    public Task AddGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/usergroup/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the AddGroup operation.
    /// </summary>
    public void AddGroup(string groupName)
        => Send(HttpMethod.Post, $"security/usergroup/group/{Encode(groupName)}");

    /// <summary>
    /// Executes the DeleteGroupAsync operation.
    /// </summary>
    public Task DeleteGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/usergroup/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the DeleteGroup operation.
    /// </summary>
    public void DeleteGroup(string groupName)
        => Send(HttpMethod.Delete, $"security/usergroup/group/{Encode(groupName)}");

    /// <summary>
    /// Executes the GetUsersForGroupAsync operation.
    /// </summary>
    public Task<UsersListResponse> GetUsersForGroupAsync(string groupName, CancellationToken cancellationToken = default)
        => SendAsync<UsersListResponse>(HttpMethod.Get, $"security/usergroup/group/{Encode(groupName)}/users", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetUsersForGroup operation.
    /// </summary>
    public UsersListResponse GetUsersForGroup(string groupName)
        => Send<UsersListResponse>(HttpMethod.Get, $"security/usergroup/group/{Encode(groupName)}/users");

    /// <summary>
    /// Executes the GetGroupsForUserAsync operation.
    /// </summary>
    public Task<GroupsListResponse> GetGroupsForUserAsync(string userName, CancellationToken cancellationToken = default)
        => SendAsync<GroupsListResponse>(HttpMethod.Get, $"security/usergroup/user/{Encode(userName)}/groups", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the GetGroupsForUser operation.
    /// </summary>
    public GroupsListResponse GetGroupsForUser(string userName)
        => Send<GroupsListResponse>(HttpMethod.Get, $"security/usergroup/user/{Encode(userName)}/groups");

    /// <summary>
    /// Executes the AssociateUserWithGroupAsync operation.
    /// </summary>
    public Task AssociateUserWithGroupAsync(string userName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the AssociateUserWithGroup operation.
    /// </summary>
    public void AssociateUserWithGroup(string userName, string groupName)
        => Send(HttpMethod.Post, $"security/usergroup/user/{Encode(userName)}/group/{Encode(groupName)}");

    /// <summary>
    /// Executes the RemoveUserFromGroupAsync operation.
    /// </summary>
    public Task RemoveUserFromGroupAsync(string userName, string groupName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"security/usergroup/user/{Encode(userName)}/group/{Encode(groupName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Executes the RemoveUserFromGroup operation.
    /// </summary>
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
