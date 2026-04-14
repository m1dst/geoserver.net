using Newtonsoft.Json;

namespace GeoServer.Models.Workspaces;

/// <summary>
/// Request payload used to create a workspace.
/// </summary>
public sealed class WorkspaceCreateRequest
{
    /// <summary>
    /// Workspace name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
