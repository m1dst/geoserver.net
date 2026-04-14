using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoServer.Models.About;

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

    /// <summary>
    /// Backward-compatible alias for status payloads that return <c>statuss</c>.
    /// </summary>
    [JsonProperty("statuss")]
    private JObject? StatusAlias
    {
        set
        {
            if (value is null)
            {
                return;
            }

            var statusItems = value["status"] as JArray;
            if (statusItems is null)
            {
                return;
            }

            var mapped = new List<AboutResourceDto>();
            foreach (var item in statusItems)
            {
                if (item is not JObject obj)
                {
                    continue;
                }

                mapped.Add(new AboutResourceDto
                {
                    Name = (string?)obj["name"],
                    Href = (string?)obj["href"]
                });
            }

            if (mapped.Count > 0)
            {
                About.Resources = mapped;
            }
        }
    }
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

    /// <summary>
    /// Backward-compatible alias for payloads using <c>resources</c>.
    /// </summary>
    [JsonProperty("resources")]
    private List<AboutResourceDto>? ResourcesAlias
    {
        set
        {
            if (value is not null && value.Count > 0)
            {
                Resources = value;
            }
        }
    }

    /// <summary>
    /// Backward-compatible alias for payloads using <c>status</c> arrays.
    /// </summary>
    [JsonProperty("status")]
    private List<AboutResourceDto>? StatusAlias
    {
        set
        {
            if (value is not null && value.Count > 0)
            {
                Resources = value;
            }
        }
    }
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
    /// Backward-compatible alias for payloads using <c>@name</c>.
    /// </summary>
    [JsonProperty("@name")]
    private string? NameAlias
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                Name = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the Manifest value.
    /// </summary>
    [JsonProperty("manifest")]
    public string? Manifest { get; set; }

    /// <summary>
    /// Gets or sets the Href value.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the Available value.
    /// </summary>
    [JsonProperty("available")]
    public bool? Available { get; set; }

    /// <summary>
    /// Backward-compatible alias for payloads using <c>isAvailable</c>.
    /// </summary>
    [JsonProperty("isAvailable")]
    private bool? IsAvailableAlias
    {
        set
        {
            if (value.HasValue)
            {
                Available = value.Value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the Enabled value.
    /// </summary>
    [JsonProperty("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Backward-compatible alias for payloads using <c>isEnabled</c>.
    /// </summary>
    [JsonProperty("isEnabled")]
    private bool? IsEnabledAlias
    {
        set
        {
            if (value.HasValue)
            {
                Enabled = value.Value;
            }
        }
    }

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
    /// Backward-compatible alias for payloads using <c>Version</c>.
    /// </summary>
    [JsonProperty("Version")]
    private string? VersionAlias
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                Version = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the Git revision value.
    /// </summary>
    [JsonProperty("gitRevision")]
    public string? GitRevision { get; set; }

    /// <summary>
    /// Backward-compatible alias for payloads using <c>Git-Revision</c>.
    /// </summary>
    [JsonProperty("Git-Revision")]
    private string? GitRevisionAlias
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                GitRevision = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the build timestamp value.
    /// </summary>
    [JsonProperty("buildTimestamp")]
    public string? BuildTimestamp { get; set; }

    /// <summary>
    /// Backward-compatible alias for payloads using <c>Build-Timestamp</c>.
    /// </summary>
    [JsonProperty("Build-Timestamp")]
    private string? BuildTimestampAlias
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                BuildTimestamp = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
