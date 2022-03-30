#!/usr/bin/env pwsh

[CmdletBinding()]
param (
    [string]$Environment,
    [string]$Location,
    [string]$OrgId
)

$appName = 'codefirsttwins'
$roleName = 'Azure Digital Twins Data Owner'

$rgName = "rg-$appName-$Environment-001".ToLowerInvariant()
$rg = Get-AzResourceGroup -Name $rgName

$dtName = "dt-$appName-$OrgId-$Environment".ToLowerInvariant()

$contextAccount = (Get-AzContext).Account

Write-Verbose "Creating digital twins service $dtName in resource group $rgName"

New-AzDigitalTwinsInstance -ResourceName $dtName `
  -ResourceGroupName $rgName -Location $rg.Location -Tag $rg.Tags

Write-Verbose "Assigning data permissions to digital twins service $dtName to $contextAccount"

$user = Get-AzADUser -UserPrincipalName $contextAccount
$dti = Get-AzDigitalTwinsInstance -ResourceGroupName $rgName -ResourceName $dtName

New-AzRoleAssignment -ObjectId $user.Id -RoleDefinitionName $roleName -Scope $dti.Id

