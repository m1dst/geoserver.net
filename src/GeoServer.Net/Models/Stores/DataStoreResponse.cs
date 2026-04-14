using Newtonsoft.Json;

namespace GeoServer.Models.Stores;

/// <summary>
/// Single data store response wrapper.
/// </summary>
public sealed class DataStoreResponse
{
    /// <summary>
    /// Data store object.
    /// </summary>
    [JsonProperty("dataStore")]
    public DataStoreDto DataStore { get; set; } = new();
}
