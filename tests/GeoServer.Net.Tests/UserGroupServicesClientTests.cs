using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the UserGroupServicesClientTests type.
/// </summary>
public sealed class UserGroupServicesClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_ParsesWrappedAndArrayPayloads operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ParsesWrappedAndArrayPayloads()
    {
        var call = 0;
        var (client, handler) = GeoServerClientFactory.Create(_ =>
        {
            call++;
            if (call == 1)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(@"{""userGroupService"":[{""name"":""default"",""className"":""org.example.Default""}]}", System.Text.Encoding.UTF8, "application/json")
                };
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(@"[{""name"":""ldap"",""className"":""org.example.Ldap""}]", System.Text.Encoding.UTF8, "application/json")
            };
        });

        using (client)
        {
            var wrapped = await client.UserGroupServices.GetAllAsync();
            var array = client.UserGroupServices.GetAll();

            Assert.Single(wrapped.UserGroupServices);
            Assert.Equal("default", wrapped.UserGroupServices[0].Name);

            Assert.Single(array.UserGroupServices);
            Assert.Equal("ldap", array.UserGroupServices[0].Name);
        }

        Assert.Equal("/geoserver/rest/security/usergroupservices", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/security/usergroupservices", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the GetAllAsync_ParsesXmlPayload operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ParsesXmlPayload()
    {
        var xml = @"<userGroupService><userGroupService><name>default</name><className>org.example.Default</className></userGroupService></userGroupService>";
        var (client, _) = GeoServerClientFactory.Create(_ => new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml")
        });

        using (client)
        {
            var list = await client.UserGroupServices.GetAllAsync();
            Assert.Single(list.UserGroupServices);
            Assert.Equal("default", list.UserGroupServices[0].Name);
        }
    }

    /// <summary>
    /// Executes the CrudAsync_UsesExpectedRoutesAndVerbs operation.
    /// </summary>
    [Fact]
    public async Task CrudAsync_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""org.geoserver.security.xml.XMLUserGroupServiceConfig"":{""name"":""default""}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.UserGroupServices.GetByNameAsync("default");
            await client.UserGroupServices.CreateAsync(new
            {
                org_geoserver_security_xml_XMLUserGroupServiceConfig = new
                {
                    name = "default2",
                    className = "org.geoserver.security.xml.XMLUserGroupService"
                }
            });
            await client.UserGroupServices.UpdateAsync("default2", new { name = "default2" });
            await client.UserGroupServices.DeleteAsync("default2");
        }

        Assert.Equal("/geoserver/rest/security/usergroupservices/default", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }

    /// <summary>
    /// Executes the GetByNameAsync_WithXmlPayload_ReturnsRawXmlEntry operation.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_WithXmlPayload_ReturnsRawXmlEntry()
    {
        var xml = @"<org.geoserver.security.xml.XMLUserGroupServiceConfig><name>default</name></org.geoserver.security.xml.XMLUserGroupServiceConfig>";
        var (client, _) = GeoServerClientFactory.Create(_ => new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml")
        });

        using (client)
        {
            var response = await client.UserGroupServices.GetByNameAsync("default");
            Assert.True(response.Payload.ContainsKey("rawXml"));
        }
    }
}
