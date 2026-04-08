using System;
using System.Linq;
using System.Threading.Tasks;
using geoserver.net.Models.Workspaces;
using Xunit;

namespace GeoServer.Net.IntegrationTests;

public sealed class WorkspaceIntegrationTests : IClassFixture<GeoServerIntegrationFixture>
{
    private readonly GeoServerIntegrationFixture _fixture;

    public WorkspaceIntegrationTests(GeoServerIntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task WorkspaceCrud_WorksAgainstLiveServer()
    {
        var wsName = "ws" + Guid.NewGuid().ToString("N").Substring(0, 8);

        using var client = _fixture.CreateClient();
        await client.Workspaces.CreateAsync(new WorkspaceCreateRequest { Name = wsName });
        var list = await client.Workspaces.GetAllAsync();
        Assert.Contains(list.Workspaces.Workspace, w => string.Equals(w.Name, wsName, StringComparison.Ordinal));

        var fetched = await client.Workspaces.GetByNameAsync(wsName);
        Assert.Equal(wsName, fetched.Workspace.Name);

        client.Workspaces.Delete(wsName);
        var afterDelete = await client.Workspaces.GetAllAsync();
        Assert.DoesNotContain(afterDelete.Workspaces.Workspace, w => string.Equals(w.Name, wsName, StringComparison.Ordinal));
    }
}
