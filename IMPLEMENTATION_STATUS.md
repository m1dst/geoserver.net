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

- About, manifests, system-status
- Fonts
- Reload, logging, monitoring
- Resource APIs
- URL checks
- WMS/WMTS stores and layers
- Templates and transforms
- GeoWebCache REST APIs
- Importer extension APIs

## Test Status

- Unit tests: Implemented for each method in every currently implemented endpoint client.
- Integration tests: Live GeoServer workspace CRUD flow implemented.

## Next Expansion Priorities

1. WMS/WMTS stores and layers.
2. Reload, logging, monitoring.
3. URL checks and resource APIs.
4. Fonts and templates/transforms.
