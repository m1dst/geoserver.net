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

    /// <summary>
    /// Lists tasks for an import.
    /// </summary>
    public Task<ImportsResponse> GetTasksAsync(string importId, string expand = "none", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists tasks for an import (synchronous).
    /// </summary>
    public ImportsResponse GetTasks(string importId, string expand = "none")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks?expand={Encode(expand)}");

    /// <summary>
    /// Creates a task for an import.
    /// </summary>
    public Task CreateTaskAsync(string importId, object taskPayload, string expand = "none", CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"imports/{Encode(importId)}/tasks?expand={Encode(expand)}", taskPayload, cancellationToken);

    /// <summary>
    /// Creates a task for an import (synchronous).
    /// </summary>
    public void CreateTask(string importId, object taskPayload, string expand = "none")
        => Send(HttpMethod.Post, $"imports/{Encode(importId)}/tasks?expand={Encode(expand)}", taskPayload);

    /// <summary>
    /// Gets a task by id.
    /// </summary>
    public Task<ImportsResponse> GetTaskAsync(string importId, string taskId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets a task by id (synchronous).
    /// </summary>
    public ImportsResponse GetTask(string importId, string taskId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}?expand={Encode(expand)}");

    /// <summary>
    /// Updates a task.
    /// </summary>
    public Task UpdateTaskAsync(string importId, string taskId, object taskPayload, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}?expand={Encode(expand)}", taskPayload, cancellationToken);

    /// <summary>
    /// Updates a task (synchronous).
    /// </summary>
    public void UpdateTask(string importId, string taskId, object taskPayload, string expand = "self")
        => Send(HttpMethod.Put, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}?expand={Encode(expand)}", taskPayload);

    /// <summary>
    /// Deletes a task.
    /// </summary>
    public Task DeleteTaskAsync(string importId, string taskId, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes a task (synchronous).
    /// </summary>
    public void DeleteTask(string importId, string taskId)
        => Send(HttpMethod.Delete, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}");

    /// <summary>
    /// Gets task progress.
    /// </summary>
    public Task<ImportsResponse> GetTaskProgressAsync(string importId, string taskId, CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/progress", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets task progress (synchronous).
    /// </summary>
    public ImportsResponse GetTaskProgress(string importId, string taskId)
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/progress");

    /// <summary>
    /// Gets task target store.
    /// </summary>
    public Task<ImportsResponse> GetTaskTargetAsync(string importId, string taskId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/target?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets task target store (synchronous).
    /// </summary>
    public ImportsResponse GetTaskTarget(string importId, string taskId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/target?expand={Encode(expand)}");

    /// <summary>
    /// Updates task target store.
    /// </summary>
    public Task UpdateTaskTargetAsync(string importId, string taskId, object targetPayload, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/target", targetPayload, cancellationToken);

    /// <summary>
    /// Updates task target store (synchronous).
    /// </summary>
    public void UpdateTaskTarget(string importId, string taskId, object targetPayload)
        => Send(HttpMethod.Put, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/target", targetPayload);

    /// <summary>
    /// Gets task target layer.
    /// </summary>
    public Task<ImportsResponse> GetTaskLayerAsync(string importId, string taskId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/layer?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets task target layer (synchronous).
    /// </summary>
    public ImportsResponse GetTaskLayer(string importId, string taskId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/layer?expand={Encode(expand)}");

    /// <summary>
    /// Updates task target layer.
    /// </summary>
    public Task UpdateTaskLayerAsync(string importId, string taskId, object layerPayload, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/layer?expand={Encode(expand)}", layerPayload, cancellationToken);

    /// <summary>
    /// Updates task target layer (synchronous).
    /// </summary>
    public void UpdateTaskLayer(string importId, string taskId, object layerPayload, string expand = "self")
        => Send(HttpMethod.Put, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/layer?expand={Encode(expand)}", layerPayload);

    /// <summary>
    /// Gets import data.
    /// </summary>
    public Task<ImportsResponse> GetImportDataAsync(string importId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/data?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets import data (synchronous).
    /// </summary>
    public ImportsResponse GetImportData(string importId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/data?expand={Encode(expand)}");

    /// <summary>
    /// Gets task data.
    /// </summary>
    public Task<ImportsResponse> GetTaskDataAsync(string importId, string taskId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets task data (synchronous).
    /// </summary>
    public ImportsResponse GetTaskData(string importId, string taskId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data?expand={Encode(expand)}");

    /// <summary>
    /// Lists import data files.
    /// </summary>
    public Task<ImportsResponse> GetImportDataFilesAsync(string importId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/data/files?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists import data files (synchronous).
    /// </summary>
    public ImportsResponse GetImportDataFiles(string importId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/data/files?expand={Encode(expand)}");

    /// <summary>
    /// Gets one import data file.
    /// </summary>
    public Task<ImportsResponse> GetImportDataFileAsync(string importId, string fileName, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/data/files/{Encode(fileName)}?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one import data file (synchronous).
    /// </summary>
    public ImportsResponse GetImportDataFile(string importId, string fileName, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/data/files/{Encode(fileName)}?expand={Encode(expand)}");

    /// <summary>
    /// Deletes one import data file.
    /// </summary>
    public Task DeleteImportDataFileAsync(string importId, string fileName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"imports/{Encode(importId)}/data/files/{Encode(fileName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes one import data file (synchronous).
    /// </summary>
    public void DeleteImportDataFile(string importId, string fileName)
        => Send(HttpMethod.Delete, $"imports/{Encode(importId)}/data/files/{Encode(fileName)}");

    /// <summary>
    /// Lists task data files.
    /// </summary>
    public Task<ImportsResponse> GetTaskDataFilesAsync(string importId, string taskId, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data/files?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists task data files (synchronous).
    /// </summary>
    public ImportsResponse GetTaskDataFiles(string importId, string taskId, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data/files?expand={Encode(expand)}");

    /// <summary>
    /// Gets one task data file.
    /// </summary>
    public Task<ImportsResponse> GetTaskDataFileAsync(string importId, string taskId, string fileName, string expand = "self", CancellationToken cancellationToken = default)
        => SendAsync<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data/files/{Encode(fileName)}?expand={Encode(expand)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Gets one task data file (synchronous).
    /// </summary>
    public ImportsResponse GetTaskDataFile(string importId, string taskId, string fileName, string expand = "self")
        => Send<ImportsResponse>(HttpMethod.Get, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data/files/{Encode(fileName)}?expand={Encode(expand)}");

    /// <summary>
    /// Deletes one task data file.
    /// </summary>
    public Task DeleteTaskDataFileAsync(string importId, string taskId, string fileName, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data/files/{Encode(fileName)}", cancellationToken: cancellationToken);

    /// <summary>
    /// Deletes one task data file (synchronous).
    /// </summary>
    public void DeleteTaskDataFile(string importId, string taskId, string fileName)
        => Send(HttpMethod.Delete, $"imports/{Encode(importId)}/tasks/{Encode(taskId)}/data/files/{Encode(fileName)}");

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
