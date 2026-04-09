using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace geoserver.net.Models.Importer;

/// <summary>
/// Typed list response for importer contexts.
/// </summary>
public sealed class ImporterListResponse
{
    /// <summary>
    /// Gets or sets the Imports value.
    /// </summary>
    [JsonProperty("imports")]
    public List<ImporterSummaryDto> Imports { get; set; } = new();
}

/// <summary>
/// Typed single importer context response.
/// </summary>
public sealed class ImporterContextResponse
{
    /// <summary>
    /// Gets or sets the Import value.
    /// </summary>
    [JsonProperty("import")]
    public ImporterContextDto Import { get; set; } = new();
}

/// <summary>
/// Typed list response for importer tasks.
/// </summary>
public sealed class ImporterTasksResponse
{
    /// <summary>
    /// Gets or sets the Tasks value.
    /// </summary>
    [JsonProperty("tasks")]
    public List<ImporterTaskDto> Tasks { get; set; } = new();
}

/// <summary>
/// Typed single importer task response.
/// </summary>
public sealed class ImporterTaskResponse
{
    /// <summary>
    /// Gets or sets the Task value.
    /// </summary>
    [JsonProperty("task")]
    public ImporterTaskDto Task { get; set; } = new();
}

/// <summary>
/// Typed list response for importer task transforms.
/// </summary>
public sealed class ImporterTransformsResponse
{
    /// <summary>
    /// Gets or sets the Transforms value.
    /// </summary>
    [JsonProperty("transforms")]
    public List<ImporterTransformDto> Transforms { get; set; } = new();
}

/// <summary>
/// Typed single importer task transform response.
/// </summary>
public sealed class ImporterTransformResponse
{
    /// <summary>
    /// Gets or sets the Transform value.
    /// </summary>
    [JsonProperty("transform")]
    public ImporterTransformDto Transform { get; set; } = new();
}

/// <summary>
/// Import summary DTO.
/// </summary>
public sealed class ImporterSummaryDto
{
    /// <summary>
    /// Gets or sets the Id value.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the State value.
    /// </summary>
    [JsonProperty("state")]
    public string? State { get; set; }

    /// <summary>
    /// Gets or sets the Href value.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }
}

/// <summary>
/// Import context DTO.
/// </summary>
public sealed class ImporterContextDto
{
    /// <summary>
    /// Gets or sets the Id value.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the State value.
    /// </summary>
    [JsonProperty("state")]
    public string? State { get; set; }

    /// <summary>
    /// Gets or sets the Href value.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the Message value.
    /// </summary>
    [JsonProperty("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the Archive value.
    /// </summary>
    [JsonProperty("archive")]
    public bool? Archive { get; set; }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Import task DTO.
/// </summary>
public sealed class ImporterTaskDto
{
    /// <summary>
    /// Gets or sets the Id value.
    /// </summary>
    [JsonProperty("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the State value.
    /// </summary>
    [JsonProperty("state")]
    public string? State { get; set; }

    /// <summary>
    /// Gets or sets the UpdateMode value.
    /// </summary>
    [JsonProperty("updateMode")]
    public string? UpdateMode { get; set; }

    /// <summary>
    /// Gets or sets the Href value.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}

/// <summary>
/// Import transform DTO.
/// </summary>
public sealed class ImporterTransformDto
{
    /// <summary>
    /// Gets or sets the Type value.
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the Href value.
    /// </summary>
    [JsonProperty("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the AdditionalData value.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; } = new Dictionary<string, JToken>();
}
