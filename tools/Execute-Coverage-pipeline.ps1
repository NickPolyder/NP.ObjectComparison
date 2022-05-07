param(
[Parameter(Mandatory=$false)]
[string] $Tag,
[Parameter(Mandatory=$false)]
[string] $HistoryPath
)

$env:PATH = $env:PATH + ":/home/pi/.dotnet:/home/pi/.dotnet/tools";

& ./tools/Execute-Coverage.ps1 -IsPipeline -Tag $Tag -HistoryPath $HistoryPath