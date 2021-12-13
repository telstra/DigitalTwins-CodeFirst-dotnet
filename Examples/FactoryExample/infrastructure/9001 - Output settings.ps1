#!/usr/bin/env pwsh

[CmdletBinding()]
param (
    [string]$Environment,
    [string]$Location,
    [string]$OrgId
)

$appName = 'codefirsttwins'
$rgName = "rg-$appName-$Environment-001".ToLowerInvariant()
$dtName = "dt-$appName-$OrgId-$Environment".ToLowerInvariant()

Write-Verbose 'Digital Twins host name:'
(Get-AzDigitalTwinsInstance -ResourceGroupName $rgName -ResourceName $dtName).HostName

Write-Verbose 'IOT Hub host name:'
(Get-AzIotHub $rgName).Properties.HostName
