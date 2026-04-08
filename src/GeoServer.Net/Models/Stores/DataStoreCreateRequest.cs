using Newtonsoft.Json;

namespace geoserver.net.Models.Stores;

/// <summary>
/// Request payload used to create a data store.
/// </summary>
public sealed class DataStoreCreateRequest
{
    /// <summary>
    /// Data store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Data store type.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = "PostGIS";

    /// <summary>
    /// Optional enabled flag.
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }
}
