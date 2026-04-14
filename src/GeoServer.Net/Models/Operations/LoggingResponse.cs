using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoServer.Models.Operations;

/// <summary>
/// Logging configuration response.
/// </summary>
public sealed class LoggingResponse
{
    /// <summary>
    /// Typed logging payload.
    /// </summary>
    [JsonProperty("logging")]
    public LoggingConfigurationDto LoggingTyped { get; set; } = new();

    /// <summary>
    /// Backward-compatible alias for the typed logging payload.
    /// </summary>
    [JsonIgnore]
    public object? Logging
    {
        get => LoggingTyped;
        set
        {
            if (value is null)
            {
                LoggingTyped = new LoggingConfigurationDto();
                return;
            }

            if (value is LoggingConfigurationDto typed)
            {
                LoggingTyped = typed;
                return;
            }

            if (value is JObject jObject)
            {
                LoggingTyped = jObject.ToObject<LoggingConfigurationDto>() ?? new LoggingConfigurationDto();
            }
        }
    }
}

/// <summary>
/// Logging configuration payload.
/// </summary>
public sealed class LoggingConfigurationDto
{
    /// <summary>
    /// Gets or sets the Level value.
    /// </summary>
    [JsonProperty("level")]
    public string? Level { get; set; }

    /// <summary>
    /// Gets or sets the Location value.
    /// </summary>
    [JsonProperty("location")]
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets the StdOutLogging value.
    /// </summary>
    [JsonProperty("stdOutLogging")]
    public bool? StdOutLogging { get; set; }

    /// <summary>
    /// Gets or sets the JdbcLogging value.
    /// </summary>
    [JsonProperty("jdbcLogging")]
    public bool? JdbcLogging { get; set; }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
