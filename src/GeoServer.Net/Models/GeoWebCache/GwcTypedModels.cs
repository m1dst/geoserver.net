using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.GeoWebCache;

/// <summary>
/// Typed response for GeoWebCache global configuration.
/// </summary>
public sealed class GwcGlobalTypedResponse
{
    /// <summary>
    /// Global configuration payload.
    /// </summary>
    [JsonProperty("global")]
    public GwcNamedCollectionDto Global { get; set; } = new();
}

/// <summary>
/// Typed response for GeoWebCache layers payload.
/// </summary>
public sealed class GwcLayersTypedResponse
{
    /// <summary>
    /// Layers payload.
    /// </summary>
    [JsonProperty("layers")]
    public GwcNamedCollectionDto Layers { get; set; } = new();
}

/// <summary>
/// Typed response for GeoWebCache seed status payload.
/// </summary>
public sealed class GwcSeedStatusTypedResponse
{
    /// <summary>
    /// Raw seed status tuples returned by GeoWebCache.
    /// </summary>
    [JsonProperty("long-array-array")]
    public List<List<long>> LongArrayArray { get; set; } = new();

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Typed response for GeoWebCache disk quota payload.
/// </summary>
public sealed class GwcDiskQuotaTypedResponse
{
    /// <summary>
    /// Disk quota configuration payload.
    /// </summary>
    [JsonProperty("org.geowebcache.diskquota.DiskQuotaConfig")]
    public GwcNamedCollectionDto DiskQuotaConfig { get; set; } = new();

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Typed response for GeoWebCache blob stores payload.
/// </summary>
public sealed class GwcBlobStoresTypedResponse
{
    /// <summary>
    /// Blob stores payload.
    /// </summary>
    [JsonProperty("blobStores")]
    public GwcNamedCollectionDto BlobStores { get; set; } = new();
}

/// <summary>
/// Typed response for GeoWebCache grid sets payload.
/// </summary>
public sealed class GwcGridSetsTypedResponse
{
    /// <summary>
    /// Grid sets payload.
    /// </summary>
    [JsonProperty("gridSets")]
    public GwcNamedCollectionDto GridSets { get; set; } = new();
}

/// <summary>
/// Common DTO for named collections used by GeoWebCache JSON payloads.
/// </summary>
public sealed class GwcNamedCollectionDto
{
    /// <summary>
    /// Single resource name for detail-style payloads.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Resource names for list-style payloads.
    /// </summary>
    [JsonProperty("names")]
    public List<string> Names { get; set; } = new();

    /// <summary>
    /// Optional enabled flag when present in payloads.
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Optional backend timeout when present in payloads.
    /// </summary>
    [JsonProperty("backendTimeout")]
    public int? BackendTimeout { get; set; }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
