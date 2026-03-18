param(
    [string]$TestProject = ".\AsyncApiShowcase.Tests\AsyncApiShowcase.Tests.csproj",
    [switch]$OpenReport
)

$ErrorActionPreference = "Stop"

$repoRoot = $PSScriptRoot
$dotnetCliHome = Join-Path $repoRoot ".dotnet"
$env:DOTNET_CLI_HOME = $dotnetCliHome
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"

$resultsDir = Join-Path $repoRoot "TestResults"
$coverageDir = Join-Path $repoRoot "CoverageReport"
$combinedReportPath = Join-Path $coverageDir "test-and-coverage.html"

Write-Host "Running tests with coverage..."
dotnet test $TestProject --collect:"XPlat Code Coverage" --results-directory $resultsDir --logger "trx;LogFileName=TestResults.trx"

$trxPath = Get-ChildItem -Path $resultsDir -Recurse -Filter TestResults.trx | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $trxPath) {
    Write-Warning "TestResults.trx not found under $resultsDir"
}

$toolPath = Join-Path $repoRoot ".dotnet\.dotnet\tools\reportgenerator.exe"
if (Test-Path $toolPath) {
    $reportGenerator = $toolPath
} else {
    $reportGenerator = "reportgenerator"
}

# Only include your assemblies, and exclude generated/third-party code.
$assemblyFilters = "+Api*;-AsyncApiShowcase.Tests;-Microsoft.*;-System.*;-netstandard;-mscorlib"
$fileFilters = "-**/*.g.cs;-**/*.generated.cs;-**/obj/**"

Write-Host "Generating coverage summary report..."
& $reportGenerator -reports:"$resultsDir\**\coverage.cobertura.xml" -targetdir:"$coverageDir" -reporttypes:"TextSummary;HtmlSummary" -assemblyfilters:"$assemblyFilters" -filefilters:"$fileFilters"

$summaryPath = Join-Path $coverageDir "Summary.txt"
if (Test-Path $summaryPath) {
    Write-Host "\nCoverage summary:"
    Get-Content $summaryPath
} else {
    Write-Warning "Summary.txt was not generated. Check report generator output above."
}

# Build combined HTML report (test results + coverage summary in one page).
$testRows = ""
$testCounts = $null
$testDuration = ""

if ($trxPath) {
    [xml]$trx = Get-Content $trxPath.FullName
    $testCounts = $trx.TestRun.ResultSummary.Counters
    $testDuration = $trx.TestRun.Times.duration

    $tests = @{}
    foreach ($unitTest in $trx.TestRun.TestDefinitions.UnitTest) {
        $tests[$unitTest.id] = $unitTest
    }

    foreach ($result in $trx.TestRun.Results.UnitTestResult) {
        $def = $tests[$result.testId]
        $className = $def.TestMethod.className
        $testName = $def.TestMethod.name
        $outcome = $result.outcome
        $duration = $result.duration
        $testRows += "<tr><td>$className</td><td>$testName</td><td class='o-$outcome'>$outcome</td><td>$duration</td></tr>"
    }
}

$coverageIframe = "summary.html"

$combinedHtml = @"
<!doctype html>
<html>
<head>
  <meta charset="utf-8" />
  <title>Tests + Coverage</title>
  <style>
    body { font-family: Segoe UI, Arial, sans-serif; margin: 24px; }
    h1 { margin-bottom: 8px; }
    h2 { margin-top: 24px; }
    .summary { display: grid; grid-template-columns: repeat(5, max-content); gap: 8px 16px; }
    .summary div { padding: 6px 10px; background: #f3f3f3; border-radius: 6px; }
    table { border-collapse: collapse; width: 100%; margin-top: 10px; }
    td, th { border: 1px solid #ddd; padding: 6px 10px; text-align: left; }
    th { background: #f3f3f3; position: sticky; top: 0; }
    .o-Passed { color: #116329; font-weight: 600; }
    .o-Failed { color: #b42318; font-weight: 600; }
    .o-Skipped, .o-NotExecuted { color: #b54708; font-weight: 600; }
    .panel { border: 1px solid #ddd; border-radius: 8px; padding: 12px; }
    iframe { width: 100%; height: 520px; border: 1px solid #ddd; border-radius: 8px; }
  </style>
</head>
<body>
  <h1>Tests + Coverage</h1>

  <h2>Test Summary</h2>
  <div class="panel">
    <div class="summary">
      <div>Total: $($testCounts.total)</div>
      <div>Passed: $($testCounts.passed)</div>
      <div>Failed: $($testCounts.failed)</div>
      <div>Skipped: $($testCounts.notExecuted)</div>
      <div>Duration: $testDuration</div>
    </div>
    <table>
      <thead>
        <tr><th>Class</th><th>Test</th><th>Outcome</th><th>Duration</th></tr>
      </thead>
      <tbody>
        $testRows
      </tbody>
    </table>
  </div>

  <h2>Coverage Summary</h2>
  <iframe src="$coverageIframe" title="Coverage Summary"></iframe>
</body>
</html>
"@

$combinedHtml | Set-Content $combinedReportPath

if ($OpenReport) {
    if (Test-Path $combinedReportPath) {
        Start-Process $combinedReportPath
    } else {
        Write-Warning "Combined report was not generated."
    }
}
