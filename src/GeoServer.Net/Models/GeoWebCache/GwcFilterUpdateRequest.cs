using Newtonsoft.Json;

namespace GeoServer.Models.GeoWebCache;

/// <summary>
/// Request payload for GeoWebCache filter update endpoint.
/// </summary>
public sealed class GwcFilterUpdateRequest
{
    /// <summary>
    /// Projection used by the request filter.
    /// </summary>
    [JsonProperty("gridSet")]
    public string? GridSet { get; set; }

    /// <summary>
    /// Minimum zoom level where filter applies.
    /// </summary>
    [JsonProperty("zoomStart")]
    public int? ZoomStart { get; set; }

    /// <summary>
    /// Maximum zoom level where filter applies.
    /// </summary>
    [JsonProperty("zoomStop")]
    public int? ZoomStop { get; set; }
}
