using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoServer.Models.Security;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the UserGroupsClientTests type.
/// </summary>
public sealed class UserGroupsClientTests
{
    /// <summary>
    /// Executes the GetUsersAndGroupsAsync_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetUsersAndGroupsAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("/users"))
            {
                return TestHttpMessageHandler.Json(@"{""users"":[]}");
            }

            return TestHttpMessageHandler.Json(@"{""groups"":[]}");
        });

        using (client)
        {
            _ = await client.UserGroups.GetUsersAsync();
            _ = await client.UserGroups.GetGroupsAsync();
        }

        Assert.Equal("/geoserver/rest/security/usergroup/users", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/security/usergroup/groups", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the UserAndGroupMutations_UseExpectedVerbs operation.
    /// </summary>
    [Fact]
    public async Task UserAndGroupMutations_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.UserGroups.CreateUserAsync(new GeoServerUserDto { UserName = "alice", Password = "pw", Enabled = true });
            await client.UserGroups.UpdateUserAsync("alice", new GeoServerUserDto { UserName = "alice", Enabled = true });
            await client.UserGroups.DeleteUserAsync("alice");
            await client.UserGroups.AddGroupAsync("group1");
            await client.UserGroups.DeleteGroupAsync("group1");
            await client.UserGroups.AssociateUserWithGroupAsync("alice", "group1");
            await client.UserGroups.RemoveUserFromGroupAsync("alice", "group1");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[6].Method);
    }

    /// <summary>
    /// Executes the SyncQueries_UseExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void SyncQueries_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.RequestUri!.AbsolutePath.Contains("/groups"))
            {
                return TestHttpMessageHandler.Json(@"{""groups"":[]}");
            }

            return TestHttpMessageHandler.Json(@"{""users"":[]}");
        });

        using (client)
        {
            _ = client.UserGroups.GetUsers();
            _ = client.UserGroups.GetGroups();
            _ = client.UserGroups.GetUsersForGroup("group1");
            _ = client.UserGroups.GetGroupsForUser("alice");
        }

        Assert.All(handler.Requests, r => Assert.Equal(HttpMethod.Get, r.Method));
    }
}
