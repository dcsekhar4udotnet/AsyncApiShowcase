# Async API Showcase

This solution is a small, end-to-end example of an async API aggregation flow. A front-end API (ApiGateway) calls two downstream Web APIs in parallel, waits for both, and returns a single combined response. The two backend services include artificial delays so the async behavior is easy to see.

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

## How It Fits Together
1. ApiFrontend (or any HTTP client) calls ApiGateway.
2. ApiGateway fires requests to ApiService1 and ApiService2 at the same time.
3. Both services wait their configured delays, then return responses.
4. ApiGateway combines the two responses into one payload and returns it.

## Run Everything
Use the script at the repo root to start all services at once:

```powershell
.\run-all.ps1
```

Optional: use HTTPS profiles instead of HTTP:

```powershell
.\run-all.ps1 -Profile https
```
