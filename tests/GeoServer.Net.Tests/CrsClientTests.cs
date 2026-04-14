using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the CrsClientTests type.
/// </summary>
public sealed class CrsClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""crs"":[],""page"":{}}"));
        using (client)
        {
            _ = await client.Crs.GetAllAsync();
        }

        Assert.Equal("/geoserver/rest/crs.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    /// <summary>
    /// Executes the GetAll_WithFilters_AppendsQuery operation.
    /// </summary>
    [Fact]
    public void GetAll_WithFilters_AppendsQuery()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""crs"":[],""page"":{}}"));
        using (client)
        {
            _ = client.Crs.GetAll(authority: "EPSG", query: "32", offset: 10, limit: 5);
        }

        var uri = handler.Requests[0].RequestUri!;
        Assert.Equal("/geoserver/rest/crs.json", uri.AbsolutePath);
        Assert.Contains("authority=EPSG", uri.Query);
        Assert.Contains("query=32", uri.Query);
        Assert.Contains("offset=10", uri.Query);
        Assert.Contains("limit=5", uri.Query);
    }

    /// <summary>
    /// Executes the GetByIdentifierAsync_UsesEncodedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetByIdentifierAsync_UsesEncodedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""id"":""EPSG:4326"",""format"":""wkt"",""definition"":""GEOGCS[...]""}"));
        using (client)
        {
            var response = await client.Crs.GetByIdentifierAsync("EPSG:4326");
            Assert.Equal("EPSG:4326", response.Id);
        }

        Assert.Equal("/geoserver/rest/crs/EPSG%3A4326.json", handler.Requests[0].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the GetWktByIdentifierAsync_UsesWktRoute operation.
    /// </summary>
    [Fact]
    public async Task GetWktByIdentifierAsync_UsesWktRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("GEOGCS[...]", System.Text.Encoding.UTF8, "text/plain")
        });
        using (client)
        {
            var wkt = await client.Crs.GetWktByIdentifierAsync("EPSG:3857");
            Assert.Contains("GEOGCS", wkt);
        }

        Assert.Equal("/geoserver/rest/crs/EPSG%3A3857.wkt", handler.Requests[0].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the GetAuthoritiesAsync_UsesExpectedRouteAndAlias operation.
    /// </summary>
    [Fact]
    public async Task GetAuthoritiesAsync_UsesExpectedRouteAndAlias()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""authority"":[{""name"":""EPSG"",""href"":""/rest/crs?authority=EPSG""}]}"));
        using (client)
        {
            var authorities = await client.Crs.GetAuthoritiesAsync();
            Assert.Single(authorities.Authorities);
            Assert.Equal("EPSG", authorities.Authorities[0].Name);
        }

        Assert.Equal("/geoserver/rest/crs/authorities.json", handler.Requests[0].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the GetAll_InvalidPaging_Throws operation.
    /// </summary>
    [Fact]
    public void GetAll_InvalidPaging_Throws()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""crs"":[],""page"":{}}"));
        using (client)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => client.Crs.GetAll(offset: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => client.Crs.GetAll(limit: 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => client.Crs.GetAll(limit: 201));
        }
    }
}
