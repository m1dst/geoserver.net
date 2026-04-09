using Newtonsoft.Json;

namespace geoserver.net.Models.Settings;

/// <summary>
/// Workspace-specific settings response.
/// </summary>
public sealed class WorkspaceSettingsResponse
{
    /// <summary>
    /// Settings payload.
    /// </summary>
    [JsonProperty("settings")]
    public object? Settings { get; set; }
}
