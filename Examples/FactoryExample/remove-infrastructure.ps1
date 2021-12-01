[CmdletBinding()]
param (
    [string]$SubscriptionId = $null,
    [string]$AppName = 'codefirsttwins',
    [string]$Environment = 'demo'
)

# Pre-requisites:
#
# Running these scripts requires the following to be installed:
# * PowerShell, https://github.com/PowerShell/PowerShell
# * Azure CLI, https://docs.microsoft.com/en-us/cli/azure
#
# You also need to run `az login` to authenticate
#
# To see messages, set verbose preference before running:
#   $VerbosePreference = 'Continue'
#   ./deploy-infrastructure.ps1

$ErrorActionPreference="Stop"

$SubscriptionId = (Get-AzContext).Subscription.Id
Write-Verbose "Removing from context subscription ID $SubscriptionId"

$ResourceGroupName = "rg-$AppName-$Environment-001"

Remove-AzResourceGroup -Name $ResourceGroupName
