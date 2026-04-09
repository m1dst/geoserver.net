param(
    [string]$ComposeService = "geoserver",
    [string]$BaseUrl = "http://localhost:8080/geoserver/rest/",
    [string]$Username = "admin",
    [string]$Password = "geoserver",
    [int]$ReadyTimeoutSeconds = 180,
    [switch]$DetailedTestOutput
)

$ErrorActionPreference = "Stop"
$repoRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
$composeFile = Join-Path $repoRoot "docker-compose.yml"
$integrationProject = Join-Path $repoRoot "tests\GeoServer.Net.IntegrationTests\GeoServer.Net.IntegrationTests.csproj"

function Wait-GeoServerReady {
    param(
        [string]$Url,
        [string]$User,
        [string]$Pass,
        [int]$TimeoutSeconds
    )

    $pair = "$User`:$Pass"
    $token = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($pair))
    $headers = @{ Authorization = "Basic $token" }

    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            $response = Invoke-WebRequest -UseBasicParsing -Uri ($Url.TrimEnd("/") + "/about/status.json") -Headers $headers -TimeoutSec 10
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 500) {
                return
            }
        }
        catch {
            Start-Sleep -Seconds 2
            continue
        }

        Start-Sleep -Seconds 2
    }

    throw "Timed out waiting for GeoServer readiness at $Url"
}

Write-Host "Starting GeoServer container ($ComposeService)..."
docker compose -f $composeFile up -d $ComposeService | Out-Host

try {
    Write-Host "Waiting for GeoServer readiness..."
    Wait-GeoServerReady -Url $BaseUrl -User $Username -Pass $Password -TimeoutSeconds $ReadyTimeoutSeconds

    $env:GEOSERVER_BASE_URL = $BaseUrl
    $env:GEOSERVER_USERNAME = $Username
    $env:GEOSERVER_PASSWORD = $Password

    Write-Host "Running integration tests..."
    $testArgs = @($integrationProject, "--nologo")
    if ($DetailedTestOutput) {
        Write-Host "Detailed test output enabled (showing individual test results)."
        $testArgs += @("--logger", "console;verbosity=detailed")
    }

    dotnet test @testArgs
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }
}
finally {
    Write-Host "Stopping GeoServer container..."
    docker compose -f $composeFile down | Out-Host
}
