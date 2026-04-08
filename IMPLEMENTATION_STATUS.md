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
- Coverages and structured coverages
- Fonts
- Global settings and OWS service settings
- Reload, logging, monitoring
- Resource APIs
- Security APIs (roles, user groups, auth providers, filter chains)
- URL checks
- WMS/WMTS stores and layers
- Templates and transforms
- GeoWebCache REST APIs
- Importer extension APIs

## Test Status

- Unit tests: Implemented for each method in every currently implemented endpoint client.
- Integration tests: Live GeoServer workspace CRUD flow implemented.

## Next Expansion Priorities

1. Coverages (including structured coverage operations).
2. Settings/service configuration APIs.
3. Security APIs.
4. WMS/WMTS stores and layers.
