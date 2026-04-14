using Newtonsoft.Json;

namespace GeoServer.Models.Settings;

/// <summary>
/// Global settings response.
/// </summary>
public sealed class GlobalSettingsResponse
{
    /// <summary>
    /// Root global payload.
    /// </summary>
    [JsonProperty("global")]
    public object? Global { get; set; }
}
