#!/usr/bin/env pwsh

[CmdletBinding()]
param (
    [string]$Environment,
    [string]$Location,
    [string]$OrgId
)

$appName = 'codefirsttwins'

$rgName = "rg-$appName-$Environment-001".ToLowerInvariant()
$tags = @{ WorkloadName = 'codefirsttwins'; DataClassification = 'Non-business'; Criticality = 'Low'; `
  BusinessUnit = 'Demo'; ApplicationName = $appName; Env = $Environment }

Write-Verbose "Creating resource group $rgName in location $Location"

New-AzResourceGroup -Name $rgName -Location $Location -Tag $tags -Force
