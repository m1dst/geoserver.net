using Newtonsoft.Json;

namespace geoserver.net.Models.Settings;

/// <summary>
/// Generic settings payload wrapper.
/// </summary>
public sealed class GeoServerSettingsDto
{
    /// <summary>
    /// Raw settings object.
    /// </summary>
    [JsonProperty("settings")]
    public object? Settings { get; set; }
}
