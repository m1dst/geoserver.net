using Newtonsoft.Json;

namespace geoserver.net.Models.Settings;

/// <summary>
/// Contact settings response.
/// </summary>
public sealed class ContactSettingsResponse
{
    /// <summary>
    /// Contact payload.
    /// </summary>
    [JsonProperty("contact")]
    public object? Contact { get; set; }
}
