using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.GeoWebCache;

/// <summary>
/// Typed response for GeoWebCache global configuration.
/// </summary>
public sealed class GwcGlobalTypedResponse
{
    [JsonProperty("global")]
    public GwcNamedCollectionDto Global { get; set; } = new();
}

/// <summary>
/// Typed response for GeoWebCache layers payload.
/// </summary>
public sealed class GwcLayersTypedResponse
{
    [JsonProperty("layers")]
    public GwcNamedCollectionDto Layers { get; set; } = new();
}

/// <summary>
/// Typed response for GeoWebCache seed status payload.
/// </summary>
public sealed class GwcSeedStatusTypedResponse
{
    [JsonProperty("long-array-array")]
    public List<List<long>> LongArrayArray { get; set; } = new();

    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Typed response for GeoWebCache disk quota payload.
/// </summary>
public sealed class GwcDiskQuotaTypedResponse
{
    [JsonProperty("org.geowebcache.diskquota.DiskQuotaConfig")]
    public GwcNamedCollectionDto DiskQuotaConfig { get; set; } = new();

    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Typed response for GeoWebCache blob stores payload.
/// </summary>
public sealed class GwcBlobStoresTypedResponse
{
    [JsonProperty("blobStores")]
    public GwcNamedCollectionDto BlobStores { get; set; } = new();
}

/// <summary>
/// Typed response for GeoWebCache grid sets payload.
/// </summary>
public sealed class GwcGridSetsTypedResponse
{
    [JsonProperty("gridSets")]
    public GwcNamedCollectionDto GridSets { get; set; } = new();
}

/// <summary>
/// Common DTO for named collections used by GeoWebCache JSON payloads.
/// </summary>
public sealed class GwcNamedCollectionDto
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("names")]
    public List<string> Names { get; set; } = new();

    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }

    [JsonProperty("backendTimeout")]
    public int? BackendTimeout { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
