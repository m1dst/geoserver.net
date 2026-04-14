using System;
using System.Net.Http;
using System.Threading.Tasks;
using GeoServer.Models.Security;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the SecurityConfigClientTests type.
/// </summary>
public sealed class SecurityConfigClientTests
{
    /// <summary>
    /// Executes the GetEndpoints_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetEndpoints_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            var path = request.RequestUri!.AbsolutePath;
            if (path.EndsWith("/security/masterpw"))
            {
                return TestHttpMessageHandler.Json(@"{""oldMasterPassword"":""geoserver""}");
            }

            return TestHttpMessageHandler.Json(@"{""mode"":""HIDE""}");
        });

        using (client)
        {
            var master = await client.Security.GetMasterPasswordAsync();
            var catalog = client.Security.GetCatalogMode();

            Assert.Equal("geoserver", master.OldMasterPassword);
            Assert.Equal("HIDE", catalog.Mode);
        }

        Assert.Equal("/geoserver/rest/security/masterpw", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/security/acl/catalog", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.All(handler.Requests, request => Assert.Equal(HttpMethod.Get, request.Method));
    }

    /// <summary>
    /// Executes the UpdateEndpoints_UseExpectedVerbsAndRoutes operation.
    /// </summary>
    [Fact]
    public async Task UpdateEndpoints_UseExpectedVerbsAndRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Security.UpdateMasterPasswordAsync(new UpdateMasterPasswordRequest
            {
                OldMasterPassword = "old",
                NewMasterPassword = "new"
            });

            await client.Security.UpdateSelfPasswordAsync(new SelfPasswordRequest
            {
                NewPassword = "new-user-password"
            });

            await client.Security.UpdateCatalogModeAsync(new CatalogModeRequest
            {
                Mode = "MIXED"
            });

            await client.Security.ReloadAclCatalogAsync();
            client.Security.ReloadAclCatalog(usePost: true);
        }

        Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
        Assert.Equal("/geoserver/rest/security/masterpw", handler.Requests[0].RequestUri!.AbsolutePath);

        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Equal("/geoserver/rest/security/self/password", handler.Requests[1].RequestUri!.AbsolutePath);

        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal("/geoserver/rest/security/acl/catalog", handler.Requests[2].RequestUri!.AbsolutePath);

        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
        Assert.Equal("/geoserver/rest/security/acl/catalog/reload", handler.Requests[3].RequestUri!.AbsolutePath);

        Assert.Equal(HttpMethod.Post, handler.Requests[4].Method);
        Assert.Equal("/geoserver/rest/security/acl/catalog/reload", handler.Requests[4].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the UpdateEndpoints_WithInvalidPayload_Throw operation.
    /// </summary>
    [Fact]
    public void UpdateEndpoints_WithInvalidPayload_Throw()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            Assert.Throws<ArgumentException>(() => client.Security.UpdateMasterPassword(new UpdateMasterPasswordRequest
            {
                OldMasterPassword = string.Empty,
                NewMasterPassword = "new"
            }));

            Assert.Throws<ArgumentException>(() => client.Security.UpdateSelfPassword(new SelfPasswordRequest
            {
                NewPassword = " "
            }));

            Assert.Throws<ArgumentException>(() => client.Security.UpdateCatalogMode(new CatalogModeRequest
            {
                Mode = ""
            }));
        }
    }
}
