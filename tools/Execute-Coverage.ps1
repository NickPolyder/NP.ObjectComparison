<#
.SYNOPSIS
A tool that executes the code coverage tools for the projects

.PARAMETER IsPipeline
A switch that determines if the script is being called from a pipeline.

.PARAMETER Tag
A Tag value for the Report Generator

.PARAMETER HistoryPath
The History Path for the Report Generator

.NOTES

Uses: https://github.com/coverlet-coverage/coverlet and  https://github.com/danielpalme/ReportGenerator
#>
param(
[Parameter(Mandatory=$false)]
[switch] $IsPipeline,
[Parameter(Mandatory=$false)]
[string] $Tag,
[Parameter(Mandatory=$false)]
[string] $HistoryPath
)
$Path = [System.IO.Path]::Combine((Split-Path $PSScriptRoot),"Tests")

$Configuration = 'Debug';

$separator = [System.IO.Path]::DirectorySeparatorChar;
$ExcludeByAttributes = @('Obsolete', 'GeneratedCode', 'CompilerGenerated', 'ExcludeFromCodeCoverage');

$ExcludeByFiles = @('**/*.generated.cs', '**/*.Designer.cs');

$separatorRegex = $separator;

if([System.Environment]::OSVersion.Platform -eq 'Win32NT')
{
   $separatorRegex =  $separator + $separator;
}

$regexConfiguration = '^*.' + $separatorRegex + 'bin' + $separatorRegex + $Configuration + '*.';

$testProjects = (Get-ChildItem $Path -Include *Tests.csproj -Recurse -Force);

$length = ($testProjects | Measure-Object).Count
$activityName = "Run coverage";

if($length -le 0)
{
    Write-Output 'No scripts to run.'
    return;
}

$coverletCoveragePath = $PSScriptRoot + $separator + 'coverage' + $separator;
$coverageLogFile = $PSScriptRoot  + $separator + 'coverage.log'
$reportGeneratorResult = $PSScriptRoot + $separator + 'coverageReport' + $separator;
$testResultsDirectory = $PSScriptRoot + $separator + 'unitTestResults';

if(Test-Path $coverletCoveragePath)
{
    Remove-Item $coverletCoveragePath -Force -Recurse
}

if(Test-Path $coverageLogFile)
{
    Remove-Item $coverageLogFile -Force
}

if(Test-Path $testResultsDirectory)
{
    Remove-Item $testResultsDirectory -Force -Recurse
}

Write-Progress -Activity $activityName -Status 'Progress->' -PercentComplete 0 -CurrentOperation "Starting..."

$commandBuilder = [System.Text.StringBuilder]::new();

$coverageFiles = @()

for($index = 0; $index -lt $length; $index++)
{	
	$project = $testProjects[$index];
	
	Write-Progress -Activity $activityName -Status 'Progress->' -PercentComplete (($index * 100) / $length) -CurrentOperation $project.Name
	
    $dllName = $project.Name -replace $project.Extension, '.dll';
	
	$dllPath = (Get-ChildItem $project.Directory.FullName -Include $dllName -Recurse -Force `
    | Where-Object { $_.Directory.FullName -Match $regexConfiguration } `
    | Where-Object { $_.Directory.FullName -NotMatch 'ref' } `
    | Select-Object -First 1).FullName;
             
    if($null -ne $dllPath -And -Not (Test-Path $dllPath))
    {
        Write-Output ''
        Write-Warning 'Cannot find dll for:' $project.Name
        continue;
    }

	
    [void]$commandBuilder.Append('& coverlet ' + $dllPath);
    [void]$commandBuilder.Append(' --target "dotnet" --targetargs "test ' + $project.FullName + ' --no-build')
    [void]$commandBuilder.Append(' -c ' + $Configuration);
    [void]$commandBuilder.Append(' -r ' + $testResultsDirectory);	
	
	$operation = ($project.Name -replace $project.Extension, '');
		
    [void]$commandBuilder.Append(' --logger:trx;LogFileName=tests.' + $operation + '.trx');
    [void]$commandBuilder.Append(' "');

	foreach($item in $ExcludeByAttributes)
    {
        [void]$commandBuilder.Append(' --exclude-by-attribute "' + $item + '"');
    }

	foreach($item in $ExcludeByFiles)
    {
        [void]$commandBuilder.Append(' --exclude-by-file "' + $item + '"');
    }
	
	$coverageXml = $coverletCoveragePath + $operation + '.xml'
	$coverageFiles += $coverageXml;
	[void]$commandBuilder.Append(' --output ' + $coverageXml);
	[void]$commandBuilder.Append(' --format cobertura ');
	
    $command = $commandBuilder.ToString();
    if($IsPipeline -eq $true)
    {
        Invoke-Expression $command
    }else{
        Invoke-Expression $command | Out-File $coverageLogFile -Append
    }

    [void]$commandBuilder.Clear();
}


if(Test-Path $reportGeneratorResult)
{
    Remove-Item $reportGeneratorResult -Force -Recurse
}

$reportsArg = [string]::Join(";", $coverageFiles)
    
$sourcePath = [System.IO.Path]::Combine((Split-Path $PSScriptRoot),"src")

if($IsPipeline -eq $true)
{
    & reportgenerator -reports:$reportsArg -targetdir:$reportGeneratorResult -sourcedirs:$sourcePath -reporttypes:'Badges' -historydir:$HistoryPath -tag:$Tag
}else{

    & reportgenerator -reports:$reportsArg -targetdir:$reportGeneratorResult `
      -sourcedirs:$sourcePath -reporttypes:'Html_Dark;Badges' -historydir:$HistoryPath -tag:$Tag | Out-File $coverageLogFile -Append
	$startIndex = $reportGeneratorResult + 'index.htm';
	& Start-Process $startIndex
}