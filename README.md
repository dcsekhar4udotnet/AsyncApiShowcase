# Async API Showcase

This solution is a small, end-to-end example of an async API aggregation flow. A front-end API (ApiGateway) calls two downstream Web APIs in parallel, waits for both, and returns a single combined response. The two backend services include artificial delays so the async behavior is easy to see.

## Tech Stack, Packages, And Tools
- .NET 9
- ASP.NET Core Web API
- Blazor (server-rendered Razor Components)
- xUnit (`xunit`, `xunit.runner.visualstudio`)
- `Microsoft.NET.Test.Sdk`
- Coverlet (`coverlet.collector`)
- ReportGenerator (`dotnet-reportgenerator-globaltool`)

## Solution Objectives
- Demonstrate a Web API that aggregates data from two other Web APIs.
- Run the downstream calls asynchronously in parallel.
- Include artificial delays to mimic long-running work in the backend services.
- Expose both GET and POST endpoints on the front-end API.

## Projects

### ApiGateway
The front-end Web API. It exposes `GET /api/aggregate` and `POST /api/aggregate`, calls ApiService1 and ApiService2 in parallel, and returns a single combined payload. This is the main example of async aggregation.

### ApiService1
Backend Web API #1. It returns a simple response after a short artificial delay. The GET and POST endpoints are intentionally minimal to keep the focus on the aggregation flow.

### ApiService2
Backend Web API #2. Similar to ApiService1 but with a longer artificial delay so you can observe the parallel call behavior more clearly.

### ApiFrontend
A small Blazor UI that calls the ApiGateway GET/POST endpoints and shows the aggregated results, including total elapsed time. It’s a simple way to verify the async flow without using a separate API client.

## The Flow
1. ApiFrontend (or any HTTP client) calls ApiGateway.
2. ApiGateway fires requests to ApiService1 and ApiService2 at the same time.
3. Both services wait their configured delays, then return responses.
4. ApiGateway combines the two responses into one payload and returns it.

## Run Everything
Use the script at the repo root to start all services at once:

Windows (one action, no execution policy prompt):

```powershell
.\run-all.cmd
```

PowerShell script alternative (if you prefer it):

```powershell
.\run-all.ps1
```

If your system blocks unsigned scripts, you can unblock just this file once:

```powershell
Unblock-File .\run-all.ps1
```

## Unit Tests And Coverage
Unit tests live in `AsyncApiShowcase.Tests` and use xUnit with the .NET test SDK. Code coverage is collected via Coverlet and summarized with ReportGenerator.

### Packages And Tools
- `xunit`
- `xunit.runner.visualstudio`
- `Microsoft.NET.Test.Sdk`
- `coverlet.collector`
- `dotnet-reportgenerator-globaltool` (installed as a .NET tool)

### Run Tests And Generate Coverage
Use the helper script to run tests, collect coverage, and build a combined HTML report:

Windows (one action, no execution policy prompt):

```powershell
.\run-coverage.cmd -OpenReport
```

PowerShell script alternative (if you prefer it):

```powershell
.\run-coverage.ps1 -OpenReport
```

Outputs:
- `CoverageReport\test-and-coverage.html` (combined test results + coverage summary)
- `CoverageReport\summary.html` (coverage summary only)
- `CoverageReport\Summary.txt` (coverage summary text)

The report filters out framework and generated code so coverage reflects your assemblies only. You can adjust filters in `run-coverage.ps1` if you want to include or exclude additional assemblies or files.
