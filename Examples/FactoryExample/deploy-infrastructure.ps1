#!/usr/bin/env pwsh

<#
.SYNOPSIS
  Deploy the Azure infrastructure for the project. 

.NOTES

  Running these scripts requires the following to be installed:
  * PowerShell, https://github.com/PowerShell/PowerShell
  * Azure PowerShell module, https://docs.microsoft.com/en-us/powershell/azure/install-az-ps
  * Azure Digital Twins module (preview installed separately)

  You also need to connect to Azure (log in), and set the desired subscripition context.

  Follow standard naming conventions from Azure Cloud Adoption Framework, 
  with an additional organisation or subscription identifier (after app name) in global names 
  to make them unique.
  https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming

  Follow standard tagging conventions from  Azure Cloud Adoption Framework.
  https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-tagging

.EXAMPLE

  Install-Module -Name Az -Scope CurrentUser -Force
  Install-Module -Name Az.DigitalTwins -Scope CurrentUser -Force
  Register-AzResourceProvider -ProviderNamespace Microsoft.DigitalTwins
  Connect-AzAccount
  Set-AzContext -SubscriptionId $SubscriptionId
  $VerbosePreference = 'Continue'
  ./deploy-infrastructure.ps1
#>
[CmdletBinding()]
param (
    ## Number of initial scripts to skip (if they have already been run)
    [int]$Skip = 0,
    ## Deployment environment, e.g. Prod, Dev, QA, Stage, Test.
    [string]$Environment = $ENV:DEPLOY_ENVIRONMENT ?? 'Dev',
    ## The Azure region where the resource is deployed.
    [string]$Location = $ENV:DEPLOY_LOCATION ?? 'australiaeast',
    ## Identifier for the organisation (or subscription) to make global names unique.
    [string]$OrgId = $ENV:DEPLOY_ORGID ?? "0x$((Get-AzContext).Subscription.Id.Substring(0,4))"
)

# Following standard naming conventions from Azure Cloud Adoption Framework
# https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming
# With an additional organisation or subscription identifier (after app name) in global names to make them unique 

# Following standard tagging conventions from  Azure Cloud Adoption Framework
# https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-tagging


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
# You also need to authenticate and set subscription you are using:
#  Connect-AzAccount
#  Set-AzContext -SubscriptionId $SubscriptionId
#
# To see messages, set verbose preference before running:
#   $VerbosePreference = 'Continue'
#   ./deploy-infrastructure.ps1

$ErrorActionPreference="Stop"

$SubscriptionId = (Get-AzContext).Subscription.Id
Write-Verbose "Using context subscription ID $SubscriptionId"

$scriptItems = Get-ChildItem "$PSScriptRoot/infrastructure" -Filter '*.ps1' `
  | Sort-Object -Property Name `
  | Select-Object -Skip $Skip

$scriptItems | ForEach-Object { Write-Verbose "Running $($_.Name)"; & $_.FullName; }

Write-Verbose "Deployment Complete"
