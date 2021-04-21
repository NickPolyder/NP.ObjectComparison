$Path = [System.IO.Path]::Combine((Split-Path $PSScriptRoot),"Tests")

$Configuration = 'Debug';

$regexConfiguration = '^*.'+$Configuration+'*.';

$testProjects = (Get-ChildItem $Path -Include *Tests.csproj -Recurse -Force);

$length = ($testProjects | measure).Count
$activityName = "Run coverage";

if($length -le 0)
{
    Write-Output 'No scripts to run.'
    return;
}
$coverageLogFile = '.\coverage.log'
$coverletCoveragePath = '.\coverage.json';
$coverageFormatPath = '.\coverage.opencover.xml';
$reportGeneratorResult = '.\cover\';
$testResultsDirectory = $PSScriptRoot +'\test-results';

if(Test-Path $coverageLogFile)
{
    Remove-Item $coverageLogFile -Force
}

if(Test-Path $coverletCoveragePath)
{
    Remove-Item $coverletCoveragePath -Force
}

if(Test-Path $coverageFormatPath)
{
    Remove-Item $coverageFormatPath -Force
}

if(Test-Path $testResultsDirectory)
{
    Remove-Item $testResultsDirectory -Force -Recurse
}

Write-Progress -Activity $activityName -Status 'Progress->' -PercentComplete 0 -CurrentOperation "Starting..."

$commandBuilder = [System.Text.StringBuilder]::new();

for($index = 0; $index -lt $length; $index++)
{
    $item = $testProjects[$index];
    
     Write-Progress -Activity $activityName -Status 'Progress->' -PercentComplete (($index * 100) / $length) -CurrentOperation $item.Name
    $dllName = $item.Name -replace $item.Extension, '.dll';

    $dllPath = (Get-ChildItem $item.Directory.FullName -Include $dllName -Recurse -Force `
    | Where-Object { $_.Directory.FullName -Match $regexConfiguration } `
    | Select -First 1).FullName;
    
    if($dllPath -ne $null -And -Not (Test-Path $dllPath))
    {
        Write-Output ''
        Write-Warning 'Cannot find dll for:' $item.Name
        continue;
    }

    [void]$commandBuilder.Append('& coverlet ' + $dllPath);
    [void]$commandBuilder.Append(' --target "dotnet" --targetargs "test ' + $item.FullName + ' --no-build')
    [void]$commandBuilder.Append(' -c '+ $Configuration);
    [void]$commandBuilder.Append(' -r ' + $testResultsDirectory);
    [void]$commandBuilder.Append(' "');   
    [void]$commandBuilder.Append(' --merge-with ' + $coverletCoveragePath);

    if($index + 1 -eq $length)
    {
        [void]$commandBuilder.Append(' --format opencover');
    }

    $command = $commandBuilder.ToString();
    
    Invoke-Expression $command | Out-File $coverageLogFile -Append

    [void]$commandBuilder.Clear();
}

& reportgenerator -reports:$coverageFormatPath -targetdir:$reportGeneratorResult | Out-File $coverageLogFile -Append

$startIndex = $reportGeneratorResult + 'index.htm';
& start $startIndex