# Implementation Status

This file tracks what is implemented in the wrapper and what remains.

## Phase 1 Scope (Implemented)

### Workspaces
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Data Stores
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Coverage Stores
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Coverages
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Structured Coverages
- `GetIndexAsync` / `GetIndex`
- `GetGranulesAsync` / `GetGranules`
- `DeleteGranulesAsync` / `DeleteGranules`
- `GetGranuleByIdAsync` / `GetGranuleById`

### Settings
- `GetGlobalAsync` / `GetGlobal`
- `UpdateGlobalAsync` / `UpdateGlobal`
- `GetContactAsync` / `GetContact`
- `UpdateContactAsync` / `UpdateContact`
- `GetWorkspaceAsync` / `GetWorkspace`
- `CreateWorkspaceAsync` / `CreateWorkspace`
- `UpdateWorkspaceAsync` / `UpdateWorkspace`
- `DeleteWorkspaceAsync` / `DeleteWorkspace`

### OWS Service Settings
- `GetGlobalAsync` / `GetGlobal` (WMS/WFS/WCS/WMTS)
- `UpdateGlobalAsync` / `UpdateGlobal` (WMS/WFS/WCS/WMTS)
- `GetWorkspaceAsync` / `GetWorkspace` (WMS/WFS/WCS/WMTS)
- `UpdateWorkspaceAsync` / `UpdateWorkspace` (WMS/WFS/WCS/WMTS)
- `DeleteWorkspaceAsync` / `DeleteWorkspace` (WMS/WFS/WCS/WMTS)

### Security Roles
- `GetAllAsync` / `GetAll`
- `GetByUserAsync` / `GetByUser`
- `GetByGroupAsync` / `GetByGroup`
- `AddRoleAsync` / `AddRole`
- `DeleteRoleAsync` / `DeleteRole`
- `AssociateRoleWithUserAsync` / `AssociateRoleWithUser`
- `RemoveRoleFromUserAsync` / `RemoveRoleFromUser`
- `AssociateRoleWithGroupAsync` / `AssociateRoleWithGroup`
- `RemoveRoleFromGroupAsync` / `RemoveRoleFromGroup`

### Security User/Group
- `GetUsersAsync` / `GetUsers`
- `CreateUserAsync` / `CreateUser`
- `UpdateUserAsync` / `UpdateUser`
- `DeleteUserAsync` / `DeleteUser`
- `GetGroupsAsync` / `GetGroups`
- `AddGroupAsync` / `AddGroup`
- `DeleteGroupAsync` / `DeleteGroup`
- `GetUsersForGroupAsync` / `GetUsersForGroup`
- `GetGroupsForUserAsync` / `GetGroupsForUser`
- `AssociateUserWithGroupAsync` / `AssociateUserWithGroup`
- `RemoveUserFromGroupAsync` / `RemoveUserFromGroup`

### Security Authentication Providers
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`
- `SetOrderAsync` / `SetOrder`

### Security Authentication Filters
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### WMS Stores
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### WMS Layers
- `GetAllAsync` / `GetAll`
- `GetAllForStoreAsync` / `GetAllForStore`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `CreateForStoreAsync` / `CreateForStore`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### WMTS Stores
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### WMTS Layers
- `GetAllAsync` / `GetAll`
- `GetAllForStoreAsync` / `GetAllForStore`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `CreateForStoreAsync` / `CreateForStore`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Operations
- `ResetAsync` / `Reset`
- `ReloadAsync` / `Reload`
- `GetLoggingAsync` / `GetLogging`
- `UpdateLoggingAsync` / `UpdateLogging`
- `GetMonitoringRequestsRawAsync` / `GetMonitoringRequestsRaw`
- `GetMonitoringRequestRawAsync` / `GetMonitoringRequestRaw`
- `ClearMonitoringRequestsAsync` / `ClearMonitoringRequests`

### Operations Typed Methods
- `GetMonitoringRequestsTypedAsync` / `GetMonitoringRequestsTyped`
- `GetMonitoringRequestTypedAsync` / `GetMonitoringRequestTyped`

### About
- `GetManifestAsync` / `GetManifest`
- `GetVersionAsync` / `GetVersion`
- `GetStatusAsync` / `GetStatus`

### About Typed Methods
- `GetManifestTypedAsync` / `GetManifestTyped`
- `GetVersionTypedAsync` / `GetVersionTyped`
- `GetStatusTypedAsync` / `GetStatusTyped`

### URL Checks
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Resources
- `GetRawAsync` / `GetRaw`
- `GetMetadataAsync` / `GetMetadata`
- `HeadAsync` / `Head`
- `PutAsync` / `Put`
- `DeleteAsync` / `Delete`

### Fonts
- `GetAllAsync` / `GetAll`

### Templates
- `GetAllAsync` / `GetAll` (scoped path support)
- `GetTemplateRawAsync` / `GetTemplateRaw`
- `PutTemplateAsync` / `PutTemplate`
- `DeleteTemplateAsync` / `DeleteTemplate`

### Transforms
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `CreateXsltAsync` / `CreateXslt`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### GeoWebCache Core
- `GetIndexRawAsync` / `GetIndexRaw`
- `ReloadAsync` / `Reload`
- `GetGlobalAsync` / `GetGlobal`
- `UpdateGlobalAsync` / `UpdateGlobal`

### GeoWebCache Layers and Seeding
- `GetLayersAsync` / `GetLayers`
- `GetLayerAsync` / `GetLayer`
- `PutLayerAsync` / `PutLayer`
- `DeleteLayerAsync` / `DeleteLayer`
- `GetSeedStatusesAsync` / `GetSeedStatuses`
- `GetLayerSeedStatusAsync` / `GetLayerSeedStatus`
- `SeedLayerAsync` / `SeedLayer`

### GeoWebCache Maintenance
- `GetMassTruncateCapabilitiesRawAsync` / `GetMassTruncateCapabilitiesRaw`
- `MassTruncateAsync` / `MassTruncate`
- `GetDiskQuotaAsync` / `GetDiskQuota`
- `UpdateDiskQuotaAsync` / `UpdateDiskQuota`

### GeoWebCache Advanced
- `GetBlobStoresAsync` / `GetBlobStores`
- `GetBlobStoreAsync` / `GetBlobStore`
- `PutBlobStoreAsync` / `PutBlobStore`
- `DeleteBlobStoreAsync` / `DeleteBlobStore`
- `GetGridSetsAsync` / `GetGridSets`
- `GetGridSetAsync` / `GetGridSet`
- `PutGridSetAsync` / `PutGridSet`
- `DeleteGridSetAsync` / `DeleteGridSet`
- `GetBoundsRawAsync` / `GetBoundsRaw`

### Importer Core
- `GetAllAsync` / `GetAll`
- `CreateAsync` / `Create`
- `DeleteAllAsync` / `DeleteAll`
- `GetByIdAsync` / `GetById`
- `CreateOrExecuteAsync` / `CreateOrExecute`
- `PutImportAsync` / `PutImport`
- `DeleteByIdAsync` / `DeleteById`

### Importer Tasks and Data
- `GetTasksAsync` / `GetTasks`
- `CreateTaskAsync` / `CreateTask`
- `GetTaskAsync` / `GetTask`
- `UpdateTaskAsync` / `UpdateTask`
- `DeleteTaskAsync` / `DeleteTask`
- `GetTaskProgressAsync` / `GetTaskProgress`
- `GetTaskTargetAsync` / `GetTaskTarget`
- `UpdateTaskTargetAsync` / `UpdateTaskTarget`
- `GetTaskLayerAsync` / `GetTaskLayer`
- `UpdateTaskLayerAsync` / `UpdateTaskLayer`
- `GetImportDataAsync` / `GetImportData`
- `GetTaskDataAsync` / `GetTaskData`
- `GetImportDataFilesAsync` / `GetImportDataFiles`
- `GetImportDataFileAsync` / `GetImportDataFile`
- `DeleteImportDataFileAsync` / `DeleteImportDataFile`
- `GetTaskDataFilesAsync` / `GetTaskDataFiles`
- `GetTaskDataFileAsync` / `GetTaskDataFile`
- `DeleteTaskDataFileAsync` / `DeleteTaskDataFile`

### Importer Transforms
- `GetTaskTransformsAsync` / `GetTaskTransforms`
- `CreateTaskTransformAsync` / `CreateTaskTransform`
- `GetTaskTransformAsync` / `GetTaskTransform`
- `UpdateTaskTransformAsync` / `UpdateTaskTransform`
- `DeleteTaskTransformAsync` / `DeleteTaskTransform`

### Importer Upload Helpers
- `UploadTaskFileAsync` / `UploadTaskFile`
- `CreateTaskFromUrlAsync` / `CreateTaskFromUrl`
- `CreateTaskMultipartAsync` / `CreateTaskMultipart`

### Importer Typed Methods
- `GetAllTypedAsync` / `GetAllTyped`
- `GetByIdTypedAsync` / `GetByIdTyped`
- `GetTasksTypedAsync` / `GetTasksTyped`
- `GetTaskTypedAsync` / `GetTaskTyped`
- `GetTaskTransformsTypedAsync` / `GetTaskTransformsTyped`
- `GetTaskTransformTypedAsync` / `GetTaskTransformTyped`

### Feature Types
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Namespaces
- `GetAllAsync` / `GetAll`
- `GetByPrefixAsync` / `GetByPrefix`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Layer Groups
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Layers
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `UpdateAsync` / `Update`
- `DeleteAsync` / `Delete`

### Styles
- `GetAllAsync` / `GetAll`
- `GetByNameAsync` / `GetByName`
- `CreateAsync` / `Create`
- `UploadSldAsync` / `UploadSld`
- `DeleteAsync` / `Delete`

## Remaining API Areas


## Test Status

- Unit tests: Implemented for each method in every currently implemented endpoint client.
- Integration tests:
  - Live GeoServer workspace CRUD flow implemented.
  - Read-only About endpoint checks implemented.
  - GeoWebCache read-only checks implemented (skip when GWC unavailable).
  - Importer read-only checks implemented (skip when Importer extension unavailable).
- CI verification:
  - Main CI builds library for both `net10.0` and `net48`.

## Next Expansion Priorities

1. Expand typed models for additional generic/raw endpoints beyond importer.
2. Add selective integration smoke tests for optional extension detection.
3. Add optional-extension aware integration checks for more endpoint groups.
4. Expand importer integration coverage beyond read-only checks.
