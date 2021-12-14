#!/usr/bin/env pwsh

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

$ErrorActionPreference="Stop"

$SubscriptionId = (Get-AzContext).Subscription.Id
Write-Verbose "Removing from context subscription ID $SubscriptionId"

$appName = 'codefirsttwins'

$rgName = "rg-$appName-$Environment-001".ToLowerInvariant()

Remove-AzResourceGroup -Name $rgName
