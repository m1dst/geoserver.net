using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using geoserver.net.Models.Importer;

namespace geoserver.net.Clients;

/// <summary>
/// Wrapper around GeoServer Importer extension core endpoints.
/// </summary>
public sealed class ImporterClient : GeoServerClientBase
{
    internal ImporterClient(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <summary>
    /// Lists imports.
    /// </summary>
    public Task<ImportsResponse> GetAllAsync(string expand = "none", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, BuildImportsPath(expand), cancellationToken: cancellationToken);

    /// <summary>
    /// Lists imports (synchronous).
    /// </summary>
    public ImportsResponse GetAll(string expand = "none")
        => Send<ImportsResponse>(HttpMethod.Get, BuildImportsPath(expand));

    /// <summary>
    /// Creates an import.
    /// </summary>
    public Task CreateAsync(object importPayload, bool exec = false, bool runAsync = false, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, BuildImportsCreatePath(exec, runAsync, expand), importPayload, cancellationToken);

    /// <summary>
    /// Creates an import (synchronous).
    /// </summary>
    public void Create(object importPayload, bool exec = false, bool runAsync = false, string expand = "self")
        => Send(HttpMethod.Post, BuildImportsCreatePath(exec, runAsync, expand), importPayload);

    /// <summary>
    /// Deletes all non-complete imports.
    /// </summary>
    public Task DeleteAllAsync(CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, "imports", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes all non-complete imports (synchronous).
    /// </summary>
    public void DeleteAll()
        => Send(HttpMethod.Delete, "imports");

    /// <summary>
    /// Gets an import by id.
    /// </summary>
    public Task<ImportsResponse> GetByIdAsync(string importId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, BuildImportByIdPath(importId, expand), cancellationToken: cancellationToken);

    /// <summary>
    /// Gets an import by id (synchronous).
    /// </summary>
    public ImportsResponse GetById(string importId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, BuildImportByIdPath(importId, expand));

    /// <summary>
    /// Creates a specific import id or executes an existing import id.
    /// </summary>
    public Task CreateOrExecuteAsync(string importId, object importPayload, bool exec = false, bool runAsync = false, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, BuildImportCreateOrExecPath(importId, exec, runAsync, expand), importPayload, cancellationToken);

    /// <summary>
    /// Creates a specific import id or executes an existing import id (synchronous).
    /// </summary>
    public void CreateOrExecute(string importId, object importPayload, bool exec = false, bool runAsync = false, string expand = "self")
        => Send(HttpMethod.Post, BuildImportCreateOrExecPath(importId, exec, runAsync, expand), importPayload);

    /// <summary>
    /// Creates a new import with next id >= importId.
    /// </summary>
    public Task PutImportAsync(string importId, object importPayload, bool exec = false, bool runAsync = false, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, BuildImportCreateOrExecPath(importId, exec, runAsync, expand), importPayload, cancellationToken);

    /// <summary>
    /// Creates a new import with next id >= importId (synchronous).
    /// </summary>
    public void PutImport(string importId, object importPayload, bool exec = false, bool runAsync = false, string expand = "self")
        => Send(HttpMethod.Put, BuildImportCreateOrExecPath(importId, exec, runAsync, expand), importPayload);

    /// <summary>
    /// Deletes an import by id.
    /// </summary>
    public Task DeleteByIdAsync(string importId, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"imports/{Encode(importId)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes an import by id (synchronous).
    /// </summary>
    public void DeleteById(string importId)
        => Send(HttpMethod.Delete, $"imports/{Encode(importId)}");

    private static string BuildImportsPath(string expand)
        => $"imports?expand={Encode(expand)}";

    private static string BuildImportsCreatePath(bool exec, bool runAsync, string expand)
        => $"imports?exec={exec.ToString().ToLowerInvariant()}&async={runAsync.ToString().ToLowerInvariant()}&expand={Encode(expand)}";

    private static string BuildImportByIdPath(string importId, string expand)
        => $"imports/{Encode(importId)}?expand={Encode(expand)}";

    private static string BuildImportCreateOrExecPath(string importId, bool exec, bool runAsync, string expand)
        => $"imports/{Encode(importId)}?exec={exec.ToString().ToLowerInvariant()}&async={runAsync.ToString().ToLowerInvariant()}&expand={Encode(expand)}";

    private static string Encode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", nameof(value));
        }

        return Uri.EscapeDataString(value);
    }
}
