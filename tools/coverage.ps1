$Path = [System.IO.Path]::Combine((Split-Path $PSScriptRoot),"Tests")

$Configuration = 'Debug';

$ExcludeByAttributes = @('Obsolete', 'GeneratedCode', 'CompilerGenerated');

$ExcludeByFiles = @('**/*.generated.cs', '**/*.Designer.cs');

$regexConfiguration = '^*.\\bin\\'+$Configuration+'*.';

$testProjects = (Get-ChildItem $Path -Include *Tests.csproj -Recurse -Force);

$length = ($testProjects | measure).Count
$activityName = "Run coverage";

if($length -le 0)
{
    Write-Output 'No scripts to run.'
    return;
}

$coverletCoveragePath = $PSScriptRoot + '\coverage\';
$coverageLogFile = $PSScriptRoot + '\coverage.log'
$reportGeneratorResult = $PSScriptRoot + '\cover\';
$testResultsDirectory = $PSScriptRoot + '\test-results';

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
    | Select -First 1).FullName;
             
    if($dllPath -ne $null -And -Not (Test-Path $dllPath))
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
    
    Invoke-Expression $command | Out-File $coverageLogFile -Append

    [void]$commandBuilder.Clear();
}

if(Test-Path $reportGeneratorResult)
{
    Remove-Item $reportGeneratorResult -Force -Recurse
}

$reportsArg = [string]::Join(";", $coverageFiles)

& reportgenerator -reports:$reportsArg -targetdir:$reportGeneratorResult | Out-File $coverageLogFile -Append
   
$startIndex = $reportGeneratorResult + 'index.htm';
& start $startIndex
