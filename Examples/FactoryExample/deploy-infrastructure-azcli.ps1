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
# * Azure CLI, https://docs.microsoft.com/en-us/cli/azure
# * IoT Extension for Azure CLI:
#     az extension add --upgrade --name azure-iot
#     az provider register --namespace 'Microsoft.DigitalTwins'
#
# You also need to run `az login` to authenticate
#
# To see messages, set verbose preference before running:
#   $VerbosePreference = 'Continue'
#   ./deploy-infrastructure.ps1

$ErrorActionPreference="Stop"

if (-not $SubscriptionId) {
  Write-Verbose "Using default subscription ID"
  $SubscriptionId = (ConvertFrom-Json "$(az account show)").id
}

# Following standard naming conventions from Azure Cloud Adoption Framework
# https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming

# Include an subscription or organisation identifier (after app name) in global names to make them unique 
$OrgId = "0x$($SubscriptionId.Substring(0,4))"

$ResourceGroupName = "rg-$AppName-$Environment-001"
$DigitalTwinsName = "dt-$AppName-$OrgId-$Environment"
$IotHubName = "iot-$AppName-$OrgId-$Environment"

# Following standard tagging conventions from  Azure Cloud Adoption Framework
# https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-tagging

$Tag = @{ WorkloadName = 'codefirsttwins'; DataClassification = 'Non-business'; Criticality = 'Low';
  BusinessUnit = 'Demo'; ApplicationName = $AppName; Env = $Environment }

# Create

az group create -g $ResourceGroupName -l $Location --subscription $SubscriptionId `
  --tags "WorkloadName=$($Tag.WorkloadName)" "DataClassification=$($Tag.DataClassification)" "Criticality=$($Tag.Criticality)" `
  "BusinessUnit=$($Tag.BusinessUnit)" "ApplicationName=$($Tag.ApplicationName)" "Env=$($Tag.Env)"

az dt create --dt-name $DigitalTwinsName --resource-group $ResourceGroupName --location $Location `
  --tags "WorkloadName=$($Tag.WorkloadName)" "DataClassification=$($Tag.DataClassification)" "Criticality=$($Tag.Criticality)" `
  "BusinessUnit=$($Tag.BusinessUnit)" "ApplicationName=$($Tag.ApplicationName)" "Env=$($Tag.Env)"

az iot hub create --name $IotHubName --resource-group $ResourceGroupName --sku S1 `
  --tags "WorkloadName=$($Tag.WorkloadName)" "DataClassification=$($Tag.DataClassification)" "Criticality=$($Tag.Criticality)" `
  "BusinessUnit=$($Tag.BusinessUnit)" "ApplicationName=$($Tag.ApplicationName)" "Env=$($Tag.Env)"

# Output

az dt show --dt-name $DigitalTwinsName
