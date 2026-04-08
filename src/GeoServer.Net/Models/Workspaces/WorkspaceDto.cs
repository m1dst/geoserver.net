using Newtonsoft.Json;

namespace geoserver.net.Models.Workspaces;

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
