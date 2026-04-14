using Newtonsoft.Json;

namespace GeoServer.Models.Workspaces;

/// <summary>
/// Single workspace response wrapper.
/// </summary>
public sealed class WorkspaceResponse
{
    /// <summary>
    /// Workspace object.
    /// </summary>
    [JsonProperty("workspace")]
    public WorkspaceDto Workspace { get; set; } = new();
}
