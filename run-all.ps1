param(
    [string]$Profile = "http"
)

$ErrorActionPreference = "Stop"

$projects = @(
    @{ Name = "ApiService1"; Path = "ApiService1\\ApiService1.csproj" },
    @{ Name = "ApiService2"; Path = "ApiService2\\ApiService2.csproj" },
    @{ Name = "ApiGateway";  Path = "ApiGateway\\ApiGateway.csproj"  },
    @{ Name = "ApiFrontend"; Path = "ApiFrontend\\ApiFrontend.csproj" }
)

Write-Host "Starting all projects with launch profile '$Profile'..."

$processes = @()
foreach ($project in $projects) {
    $args = "run --project `"$($project.Path)`" --launch-profile $Profile"
    Write-Host "Starting $($project.Name)..."
    $proc = Start-Process -FilePath "dotnet" -ArgumentList $args -NoNewWindow -PassThru
    $processes += $proc
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
