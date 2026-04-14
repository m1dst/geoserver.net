using System.Collections.Generic;
using Newtonsoft.Json;
using GeoServer.Models.Common;

namespace GeoServer.Models.Stores;

/// <summary>
/// List response for data stores.
/// </summary>
public sealed class DataStoreListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("dataStores")]
    public DataStoreListEnvelope DataStores { get; set; } = new();
}

/// <summary>
/// Data store list envelope.
/// </summary>
public sealed class DataStoreListEnvelope
{
    /// <summary>
    /// Data store entries.
    /// </summary>
    [JsonProperty("dataStore")]
    public List<NamedResourceDto> DataStore { get; set; } = new();
}
