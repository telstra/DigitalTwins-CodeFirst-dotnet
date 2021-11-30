#!/usr/bin/env pwsh
param (
  [switch]$SkipTest,
  [switch]$SkipPack
)

Write-Host 'Building Digital Twins Code First' -ForegroundColor Magenta

if (Test-Path (Join-Path $PSScriptRoot 'package')) {
  Write-Host 'Clean package folder' -ForegroundColor Magenta
  Remove-Item (Join-Path $PSScriptRoot 'package') -Recurse -Force 
}

dotnet tool restore

Write-Host 'Getting GitVersion' -ForegroundColor Magenta
$json = (dotnet tool run dotnet-gitversion /output json)
$v = ($json | ConvertFrom-Json)

Write-Host "Building version $($v.SemVer)+$($v.ShortSha) (Nuget $($v.NuGetVersion))" -ForegroundColor Cyan

if (-not $SkipTest) {
  Write-Host "Building and running unit tests" -ForegroundColor Magenta
  dotnet test (Join-Path $PSScriptRoot 'DigitalTwins-CodeFirst-dotnet.sln') -c Debug -p:AssemblyVersion=$($v.AssemblySemVer) -p:FileVersion=$($v.AssemblySemFileVer) -p:Version=$($v.SemVer)+$($v.ShortSha)
  if (!$?) { throw 'Tests failed' }
}

if (-not $SkipPack) {
  Write-Host "Packaging nuget to 'package' folder" -ForegroundColor Magenta
  dotnet pack (Join-Path $PSScriptRoot 'Telstra.Twins/Telstra.Twins.csproj') -c Release -p:AssemblyVersion=$($v.AssemblySemVer) -p:FileVersion=$($v.AssemblySemFileVer) -p:Version=$($v.SemVer)+$($v.ShortSha) -p:PackageVersion=$($v.NuGetVersion) -o (Join-Path $PSScriptRoot 'package')
  if (!$?) { throw 'Packaging failed' }
}

Write-Host "Completed" -ForegroundColor Green
