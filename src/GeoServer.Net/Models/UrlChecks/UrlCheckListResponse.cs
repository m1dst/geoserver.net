using Newtonsoft.Json;

namespace geoserver.net.Models.UrlChecks;

/// <summary>
/// Response wrapper for URL external access check collections.
/// </summary>
public sealed class UrlCheckListResponse
{
    /// <summary>
    /// URL checks payload.
    /// </summary>
    [JsonProperty("urlChecks")]
    public object? UrlChecks { get; set; }
}
