#!/usr/bin/env pwsh
[CmdletBinding()]
param (
    [string]$SubscriptionId = $null,
    [string]$AppName = 'codefirsttwins',
    [string]$Environment = 'demo',
    [string]$Location = 'australiaeast'
)

# Pre-requisites:
#
# Running these scripts requires the following to be installed:
# * PowerShell, https://github.com/PowerShell/PowerShell
# * Azure PowerShell module, https://docs.microsoft.com/en-us/powershell/azure/install-az-ps
#     Install-Module -Name Az -Scope CurrentUser -Force
# * Azure Digital Twins module (preview installed separately)
#     Install-Module -Name Az.DigitalTwins -Scope CurrentUser -Force
#     Register-AzResourceProvider -ProviderNamespace Microsoft.DigitalTwins
#
# You also need to authenticate with: Connect-AzAccount
#
# To see messages, set verbose preference before running:
#   $VerbosePreference = 'Continue'
#   ./deploy-infrastructure.ps1

$ErrorActionPreference="Stop"

if ($SubscriptionId) {
  Write-Verbose "Setting context to subscription ID $SubscriptionId"
  Set-AzContext -SubscriptionId $SubscriptionId
} else {
  $SubscriptionId = (Get-AzContext).Subscription.Id
  Write-Verbose "Using existing context subscription ID $SubscriptionId"
}

# Following standard naming conventions from Azure Cloud Adoption Framework
# https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming

# Include an subscription or organisation identifier (after app name) in global names to make them unique 
$OrgId = "x$($SubscriptionId.Substring(0,4))"

$ResourceGroupName = "rg-$AppName-$Environment-001"
$DigitalTwinsName = "dt-$AppName-$OrgId-$Environment"
$IotHubName = "iot-$AppName-$OrgId-$Environment"

# Following standard tagging conventions from  Azure Cloud Adoption Framework
# https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-tagging

$Tag = @{ WorkloadName = 'codefirsttwins'; DataClassification = 'Non-business'; Criticality = 'Low';
  BusinessUnit = 'Demo'; ApplicationName = $AppName; Env = $Environment }

# Create

New-AzResourceGroup -Name $ResourceGroupName -Location $Location -Tag $Tag -Force

New-AzDigitalTwinsInstance -ResourceGroupName $ResourceGroupName -ResourceName $DigitalTwinsName -Location $Location -Tag $Tag

New-AzIotHub -ResourceGroupName $ResourceGroupName -Name $IotHubName -SkuName S1 -Units 1 -Location $Location -Tag $Tag

# Output

(Get-AzDigitalTwinsInstance -ResourceGroupName $ResourceGroupName -ResourceName $DigitalTwinsName).HostName
(Get-AzIotHub $ResourceGroupName).Properties.HostName

