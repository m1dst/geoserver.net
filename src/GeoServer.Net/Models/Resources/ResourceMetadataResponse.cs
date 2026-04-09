using Newtonsoft.Json;

namespace geoserver.net.Models.Resources;

/// <summary>
/// JSON response wrapper for resource metadata/directory responses.
/// </summary>
public sealed class ResourceMetadataResponse
{
    /// <summary>
    /// Metadata payload for a single resource.
    /// </summary>
    [JsonProperty("ResourceMetadata")]
    public object? ResourceMetadata { get; set; }

    /// <summary>
    /// Metadata payload for a resource directory.
    /// </summary>
    [JsonProperty("ResourceDirectory")]
    public object? ResourceDirectory { get; set; }
}
