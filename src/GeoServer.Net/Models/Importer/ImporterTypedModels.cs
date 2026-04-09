using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.Importer;

/// <summary>
/// Typed list response for importer contexts.
/// </summary>
public sealed class ImporterListResponse
{
    [JsonProperty("imports")]
    public List<ImporterSummaryDto> Imports { get; set; } = new();
}

/// <summary>
/// Typed single importer context response.
/// </summary>
public sealed class ImporterContextResponse
{
    [JsonProperty("import")]
    public ImporterContextDto Import { get; set; } = new();
}

/// <summary>
/// Typed list response for importer tasks.
/// </summary>
public sealed class ImporterTasksResponse
{
    [JsonProperty("tasks")]
    public List<ImporterTaskDto> Tasks { get; set; } = new();
}

/// <summary>
/// Typed single importer task response.
/// </summary>
public sealed class ImporterTaskResponse
{
    [JsonProperty("task")]
    public ImporterTaskDto Task { get; set; } = new();
}

/// <summary>
/// Typed list response for importer task transforms.
/// </summary>
public sealed class ImporterTransformsResponse
{
    [JsonProperty("transforms")]
    public List<ImporterTransformDto> Transforms { get; set; } = new();
}

/// <summary>
/// Typed single importer task transform response.
/// </summary>
public sealed class ImporterTransformResponse
{
    [JsonProperty("transform")]
    public ImporterTransformDto Transform { get; set; } = new();
}

/// <summary>
/// Import summary DTO.
/// </summary>
public sealed class ImporterSummaryDto
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("href")]
    public string? Href { get; set; }
}

/// <summary>
/// Import context DTO.
/// </summary>
public sealed class ImporterContextDto
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("href")]
    public string? Href { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("archive")]
    public bool? Archive { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Import task DTO.
/// </summary>
public sealed class ImporterTaskDto
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("updateMode")]
    public string? UpdateMode { get; set; }

    [JsonProperty("href")]
    public string? Href { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Import transform DTO.
/// </summary>
public sealed class ImporterTransformDto
{
    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("href")]
    public string? Href { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
