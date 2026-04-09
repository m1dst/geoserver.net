using Newtonsoft.Json;

namespace geoserver.net.Models.Operations;

/// <summary>
/// Logging configuration response.
/// </summary>
public sealed class LoggingResponse
{
    /// <summary>
    /// Logging payload.
    /// </summary>
    [JsonProperty("logging")]
    public object? Logging { get; set; }
}
