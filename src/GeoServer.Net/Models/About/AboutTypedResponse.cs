using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.About;

/// <summary>
/// Typed response wrapper for GeoServer about endpoints.
/// </summary>
public sealed class AboutTypedResponse
{
    /// <summary>
    /// About payload items.
    /// </summary>
    [JsonProperty("about")]
    public AboutEnvelopeDto About { get; set; } = new();
}

/// <summary>
/// Envelope for about resources.
/// </summary>
public sealed class AboutEnvelopeDto
{
    /// <summary>
    /// Named about resources (manifest entries, versions, status items).
    /// </summary>
    [JsonProperty("resource")]
    public List<AboutResourceDto> Resources { get; set; } = new();
}

/// <summary>
/// Single about resource entry.
/// </summary>
public sealed class AboutResourceDto
{
    /// <summary>
    /// Gets or sets the Name value.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the Manifest value.
    /// </summary>
    [JsonProperty("manifest")]
    public string? Manifest { get; set; }

    /// <summary>
    /// Gets or sets the Available value.
    /// </summary>
    [JsonProperty("available")]
    public bool? Available { get; set; }

    /// <summary>
    /// Gets or sets the Enabled value.
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Gets or sets the Message value.
    /// </summary>
    [JsonProperty("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the Version value.
    /// </summary>
    [JsonProperty("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
