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

if (-not $SubscriptionId) {
  Write-Verbose "Using default subscription ID"
  $SubscriptionId = (ConvertFrom-Json "$(az account show)").id
}

$AppName = 'codefirsttwins'
$Environment = 'demo'

$ResourceGroupName = "rg-$AppName-$Environment-001"

Remove-AzResourceGroup -Name $ResourceGroupName
