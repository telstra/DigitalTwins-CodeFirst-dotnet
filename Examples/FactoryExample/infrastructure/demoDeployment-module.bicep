// Module for deploying resource group items (default target scope)

param appName string
param orgId string
param environment string
param tags object

var location = resourceGroup().location
var iotHubName = 'iot-${appName}-${orgId}-${environment}'
var digitalTwinsName = 'dt-${appName}-${orgId}-${environment}'

resource iot_appName_orgId_environment 'Microsoft.Devices/IotHubs@2021-07-01' = {
  name: iotHubName
  location: location
  tags: tags
  sku: {
    name: 'S1'
    capacity: 1
  }
}

resource dt_appName_orgId_environment 'Microsoft.DigitalTwins/digitalTwinsInstances@2020-12-01' = {
  name: digitalTwinsName
  location: location
  tags: tags
}