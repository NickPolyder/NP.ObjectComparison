<#
.SYNOPSIS
A tool that upgrades the nuget package version of the projects it finds.

.PARAMETER Path
The Path to the source folder that contains the projects to be upgraded.
[Default = ../src]

.PARAMETER CustomVersion
If set the custom version to be set.

#>

param(
[Parameter(Mandatory = $false)]
[string]$Path,
[Parameter(Mandatory = $false)]
[string]$CustomVersion)

if([System.String]::IsNullOrWhiteSpace($Path))
{
    $Path = [System.IO.Path]::Combine((Split-Path $PSScriptRoot),"src");
}

$hasCustomVersion = -not [System.String]::IsNullOrWhiteSpace($CustomVersion)

$projects = Get-ChildItem $Path -Filter "*.csproj" -Exclude ("*Tests*") -Recurse 

$length = ($projects | Measure-Object).Count

if($length -le 0)
{
    Write-Output 'No projects found.'
    return;
}

foreach($project in $projects)
{
    $xml = [xml](Get-Content $project.FullName)
    Write-Host $xml.Project.PropertyGroup.Product
    $currentVersion = [Version]::Parse($xml.Project.PropertyGroup.Version)

    if($hasCustomVersion)
    {
        $xml.Project.PropertyGroup.Version = [Version]::Parse($CustomVersion).ToString()
        $xml.Project.PropertyGroup.AssemblyVersion = $xml.Project.PropertyGroup.Version 
    }else
    {
        $hasBuildVersion = $currentVersion.Build -gt -1
        $hasRevisionVersion = $currentVersion.Revision -gt -1
        $xml.Project.PropertyGroup.Version = (GetNewVersion $currentVersion $hasRevisionVersion $hasBuildVersion).ToString()
        $xml.Project.PropertyGroup.AssemblyVersion = $xml.Project.PropertyGroup.Version     
    }
    
    Write-Host 'New Version: ' $xml.Project.PropertyGroup.AssemblyVersion
    $xml.Save($project.FullName)
}

Function GetNewVersion([Version]$currentVersion,[boolean]$hasRevision, [boolean]$hasBuildNumber)
{
    if($hasRevision)
    {
        if($currentVersion.Revision -gt 99)
        {
            if($currentVersion.Build + 1 -gt 99)
            {
                if($currentVersion.Minor + 1 -gt 99)
                {
                    return [Version]::new($currentVersion.Major + 1, 0, 0, 0)
                }

                return [Version]::new($currentVersion.Major, $currentVersion.Minor + 1 , 0, 0)
            }
            
            return [Version]::new($currentVersion.Major, $currentVersion.Minor , $currentVersion.Build + 1, 0)
        }
        
        return [Version]::new($currentVersion.Major, $currentVersion.Minor , $currentVersion.Build , $currentVersion.Revision + 1)
    }elseif($hasBuildNumber)
    {

        if($currentVersion.Build + 1 -gt 99)
        {
            if($currentVersion.Minor + 1 -gt 99)
            {
                return [Version]::new($currentVersion.Major + 1, 0, 0)
            }

            return [Version]::new($currentVersion.Major, $currentVersion.Minor + 1 , 0)
        }
        
        return [Version]::new($currentVersion.Major, $currentVersion.Minor , $currentVersion.Build + 1)
    }

     if($currentVersion.Minor + 1 -gt 99)
     {
         return [Version]::new($currentVersion.Major + 1, 0)
     }

     return [Version]::new($currentVersion.Major, $currentVersion.Minor + 1)
}