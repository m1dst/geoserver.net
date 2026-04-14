using Newtonsoft.Json;

namespace GeoServer.Models.Settings;

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
