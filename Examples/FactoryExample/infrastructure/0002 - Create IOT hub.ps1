#!/usr/bin/env pwsh

[CmdletBinding()]
param (
    [string]$Environment,
    [string]$Location,
    [string]$OrgId
)

$appName = 'codefirsttwins'
$iotSku = 'S1'
$iotUnits = 1

$rgName = "rg-$appName-$Environment-001".ToLowerInvariant()
$rg = Get-AzResourceGroup -Name $rgName

$iotName = "iot-$appName-$OrgId-$Environment".ToLowerInvariant()

Write-Verbose "Creating IOT hub $iotName in resource group $rgName"

New-AzIotHub -Name $iotName -SkuName $iotSku -Units $iotUnits `
  -ResourceGroupName $rgName -Location $rg.Location -Tag $rg.Tags
