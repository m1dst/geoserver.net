using Newtonsoft.Json;

namespace geoserver.net.Models.Namespaces;

/// <summary>
/// Namespace representation.
/// </summary>
public sealed class NamespaceDto
{
    /// <summary>
    /// Namespace prefix.
    /// </summary>
    [JsonProperty("prefix")]
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// Namespace URI.
    /// </summary>
    [JsonProperty("uri")]
    public string Uri { get; set; } = string.Empty;
}
