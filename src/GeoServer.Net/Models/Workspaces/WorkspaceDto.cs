using Newtonsoft.Json;

namespace GeoServer.Models.Workspaces;

/// <summary>
/// Workspace representation.
/// </summary>
public sealed class WorkspaceDto
{
    /// <summary>
    /// Workspace name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
