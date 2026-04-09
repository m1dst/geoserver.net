using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class RolesClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""roles"":[""ROLE_ADMINISTRATOR""]}"));
        using (client)
        {
            _ = await client.Roles.GetAllAsync();
            Assert.Equal("/geoserver/rest/security/roles", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetAll_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""roles"":[]}"));
        using (client)
        {
            _ = client.Roles.GetAll();
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task UserAndGroupQueries_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""roles"":[]}"));
        using (client)
        {
            _ = await client.Roles.GetByUserAsync("alice");
            _ = await client.Roles.GetByGroupAsync("group1");
        }

        Assert.Equal("/geoserver/rest/security/roles/user/alice", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/security/roles/group/group1", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task RoleMutations_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Roles.AddRoleAsync("ROLE_TEST");
            await client.Roles.AssociateRoleWithUserAsync("ROLE_TEST", "alice");
            await client.Roles.AssociateRoleWithGroupAsync("ROLE_TEST", "group1");
            await client.Roles.RemoveRoleFromUserAsync("ROLE_TEST", "alice");
            await client.Roles.RemoveRoleFromGroupAsync("ROLE_TEST", "group1");
            await client.Roles.DeleteRoleAsync("ROLE_TEST");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[5].Method);
    }

    [Fact]
    public void SyncRoleMutations_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Roles.AddRole("ROLE_TEST");
            client.Roles.AssociateRoleWithUser("ROLE_TEST", "alice");
            client.Roles.AssociateRoleWithGroup("ROLE_TEST", "group1");
            client.Roles.RemoveRoleFromUser("ROLE_TEST", "alice");
            client.Roles.RemoveRoleFromGroup("ROLE_TEST", "group1");
            client.Roles.DeleteRole("ROLE_TEST");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[5].Method);
    }
}
