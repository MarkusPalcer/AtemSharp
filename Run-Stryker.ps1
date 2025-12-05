param (
    [switch]$html = $false,
    [switch]$livehtml = $false,
    [switch]$full = $false,
    [Parameter(Mandatory = $false)]
    [string[]]$mutate
)

dotnet restore
dotnet tool restore

$Configuration = Get-Content -Raw "stryker-config.json" | ConvertFrom-Json
$Configuration = $Configuration.wrapper_config

if ($Configuration -ne $null)
{
    $PruningConfig = $Configuration.pruning

    if ($PruningConfig -ne $null)
    {
        $MaxNumberToKeep = $PruningConfig.max_number_to_keep
        if ($MaxNumberToKeep -ne $null)
        {
            Get-ChildItem "StrykerOutput" | Sort-Object LastWriteTime -Descending | Select-Object -Skip $MaxNumberToKeep | Remove-Item -Recurse
        }
    }
}

$invocation = 'dotnet stryker --reporter "progress" --reporter "json" --break-at 0'

# Add each mutate pattern to the invocation
if ($mutate) {
    foreach ($pattern in $mutate) {
        $invocation += " --mutate `"$pattern`""
    }
}

if ($livehtml)
{
    $html = $true
    $invocation = -join ($invocation, ' --open-report:html')
}

if ($html)
{
    $invocation = -join ($invocation, ' --reporter "html"')
}

if ($full)
{
}
else
{
    $invocation = -join ($invocation, ' --since:main')
}

Write-Host "$invocation"
Invoke-Expression -Command "$invocation"

$LatestFolder = Get-ChildItem "StrykerOutput" | Sort-Object LastWriteTime -Descending | Select-Object -first 1

if (-not (Test-Path $LatestFolder.FullName)) {
    $LatestFolderPath = Join-Path "StrykerOutput" $LatestFolder.Name
    if (-not (Test-Path $LatestFolderPath)) {
        throw "Could not find the Stryker output folder: $($LatestFolder.FullName) or $LatestFolderPath"
    } else {
        $LatestFolder = $LatestFolderPath
    }
} else {
    $LatestFolder = $LatestFolder.FullName
}

$JsonFile = Get-ChildItem "$LatestFolder\reports\*.json" | Select-Object -first 1

$JsonData = Get-Content -Raw $JsonFile | ConvertFrom-Json

$Files = $JsonData.files.PsObject.Properties | Where-Object { $_.MemberType -eq "NoteProperty" }

foreach ($File in $Files)
{
    $FileName = Resolve-Path -relative -path "$( $File.Name )"
    foreach ($Mutant in $File.Value.mutants)
    {
        if ($Mutant.status -eq "Survived")
        {
            Write-Output "$( $FileName ):$( $Mutant.location.start.line ) SURVIVED $( $Mutant.mutatorName )"
        }
        elseif ($Mutant.status -eq "NoCoverage")
        {
            Write-Output "$( $FileName ):$( $Mutant.location.start.line ) UNCOVERED $( $Mutant.mutatorName )"
        }
    }
}
