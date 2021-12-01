// Main template; to deploy use the following:
//  az login
//  az account set --subscription <subscription id>
//  az deployment sub create -l australiaeast -f infrastructure/main.bicep

targetScope = 'subscription'

param appName string = 'codefirsttwins'
param environment string = 'demo'
param orgId string = '0x${substring(subscription().subscriptionId, 0, 4)}'

var tags = {
  ApplicationName: appName
  WorkloadName: 'codefirsttwins'
  DataClassification: 'Non-business'
  Criticality: 'Low'
  BusinessUnit: 'Demo'
  Env: environment
}
var location = deployment().location
var rgName = 'rg-${appName}-${environment}-001'

resource rgDemoDeployment 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: rgName
  location: location
  tags: tags
  properties: {}
}

module demoDeployment './demoDeployment-module.bicep' = {
  name: 'demoDeployment'
  scope: rgDemoDeployment
  params: {
    appName: appName
    orgId: orgId
    environment: environment
    tags: tags
  }
  dependsOn: [
    rgDemoDeployment
  ]
}