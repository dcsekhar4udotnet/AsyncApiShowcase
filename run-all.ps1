param(
    [string]$Profile = "http",
    [switch]$OpenBrowser
)

$ErrorActionPreference = "Stop"

$projects = @(
    @{ Name = "ApiService1"; Path = "ApiService1\ApiService1.csproj" },
    @{ Name = "ApiService2"; Path = "ApiService2\ApiService2.csproj" },
    @{ Name = "ApiGateway";  Path = "ApiGateway\ApiGateway.csproj"  },
    @{ Name = "ApiFrontend"; Path = "ApiFrontend\ApiFrontend.csproj" }
)

function Get-FrontendUrl {
    param([string]$ProfileName)

    $launchSettingsPath = Join-Path $PSScriptRoot "ApiFrontend\Properties\launchSettings.json"
    if (-not (Test-Path $launchSettingsPath)) {
        return $null
    }

    $json = Get-Content $launchSettingsPath -Raw | ConvertFrom-Json
    $profile = $json.profiles.$ProfileName
    if (-not $profile) {
        return $null
    }

    $urls = ($profile.applicationUrl -split ";") | Where-Object { $_ }

    # Prefer http for first open to avoid initial https cert prompts.
    $httpUrl = $urls | Where-Object { $_ -like "http://*" } | Select-Object -First 1
    if ($httpUrl) {
        return $httpUrl
    }

    return $urls[0]
}

function Wait-ForUrl {
    param(
        [string]$Url,
        [int]$TimeoutSeconds = 30
    )

    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    while ((Get-Date) -lt $deadline) {
        try {
            Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 2 | Out-Null
            return $true
        } catch {
            Start-Sleep -Milliseconds 500
        }
    }

    return $false
}

Write-Host "Starting all projects with launch profile '$Profile'..."

$processes = @()
foreach ($project in $projects) {
    $args = "run --project `"$($project.Path)`" --launch-profile $Profile"
    Write-Host "Starting $($project.Name)..."
    $proc = Start-Process -FilePath "dotnet" -ArgumentList $args -NoNewWindow -PassThru
    $processes += $proc
}

if (-not $OpenBrowser) {
    $OpenBrowser = $true
}

if ($OpenBrowser) {
    $frontendUrl = Get-FrontendUrl -ProfileName $Profile
    if ($frontendUrl) {
        Write-Host "Waiting for ApiFrontend to be ready..."
        [void](Wait-ForUrl -Url $frontendUrl -TimeoutSeconds 30)
        Write-Host "Opening ApiFrontend in browser: $frontendUrl"
        Start-Process $frontendUrl
    }
}

Write-Host "All projects started. Press Ctrl+C to stop."
try {
    Wait-Process -Id ($processes | ForEach-Object { $_.Id })
} finally {
    Write-Host ""
    Write-Host "Stopping all projects..."
    foreach ($proc in $processes) {
        if ($proc -and -not $proc.HasExited) {
            try {
                Stop-Process -Id $proc.Id -Force
            } catch {
                # Ignore if it already exited.
            }
        }
    }
}
