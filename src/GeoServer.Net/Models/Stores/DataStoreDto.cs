using Newtonsoft.Json;

namespace GeoServer.Models.Stores;

/// <summary>
/// Data store representation.
/// </summary>
public sealed class DataStoreDto
{
    /// <summary>
    /// Store name.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Store type.
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }
}
