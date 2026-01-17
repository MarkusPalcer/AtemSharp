# Remove old reports
Get-ChildItem -Path . -Recurse -Filter coverage.cobertura.xml | Remove-Item

# Stop the script immediately on any error
$ErrorActionPreference = "Stop"

# Run tests with coverage collection
dotnet test --collect:"XPlat Code Coverage"

# Find the latest coverage.cobertura.xml file under TestResults
$coverageFiles = Get-ChildItem -Path . -Recurse -Filter coverage.cobertura.xml

if ($coverageFiles.Count -eq 0) {
    Write-Error "No coverage report files found!"
    exit 1
}

# Collect full paths of all coverage files
$coveragePaths = $coverageFiles | ForEach-Object { $_.FullName }

# Join paths with semicolon (and quote the whole string for safety)
$reportsArg = '"' + ($coveragePaths -join ";") + '"'

# Build the report output path
$targetDir = "coverage-report"
$reportType = "Html"

# Generate combined HTML report
dotnet tool restore
dotnet tool run reportgenerator `
    -reports:$reportsArg `
    "-targetdir:$targetDir" `
    "-filefilters:-*.g.cs" `
    "-reporttypes:$reportType"

New-Item -Path "$targetDir" -Name ".gitignore" -ItemType "File" -Value "*" -Force
Write-Host "Coverage report written to coverage-report\index.html"
