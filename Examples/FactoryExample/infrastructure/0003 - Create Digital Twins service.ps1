#!/usr/bin/env pwsh

[CmdletBinding()]
param (
    [string]$Environment,
    [string]$Location,
    [string]$OrgId
)

$appName = 'codefirsttwins'

$rgName = "rg-$appName-$Environment-001".ToLowerInvariant()
$rg = Get-AzResourceGroup -Name $rgName

$dtName = "dt-$appName-$OrgId-$Environment".ToLowerInvariant()

Write-Verbose "Creating digital twins service $dtName in resource group $rgName"

New-AzDigitalTwinsInstance -ResourceName $dtName `
  -ResourceGroupName $rgName -Location $rg.Location -Tag $rg.Tags
