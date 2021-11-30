@secure()
param IotHubs_iot_codefirsttwins_0xacc5_demo_connectionString string

@secure()
param IotHubs_iot_codefirsttwins_0xacc5_demo_containerName string
param IotHubs_iot_codefirsttwins_0xacc5_demo_name string
param digitalTwinsInstances_dt_codefirsttwins_0xacc5_demo_name string

resource IotHubs_iot_codefirsttwins_0xacc5_demo_name_resource 'Microsoft.Devices/IotHubs@2021-07-01' = {
  name: IotHubs_iot_codefirsttwins_0xacc5_demo_name
  location: 'australiaeast'
  tags: {
    ApplicationName: 'codefirsttwins'
    WorkloadName: 'codefirsttwins'
    DataClassification: 'Non-business'
    Criticality: 'Low'
    BusinessUnit: 'Demo'
    Env: 'demo'
  }
  sku: {
    name: 'S1'
    tier: 'Standard'
    capacity: 1
  }
  identity: {
    type: 'None'
  }
  properties: {
    ipFilterRules: []
    eventHubEndpoints: {
      events: {
        retentionTimeInDays: 1
        partitionCount: 4
      }
    }
    routing: {
      endpoints: {
        serviceBusQueues: []
        serviceBusTopics: []
        eventHubs: []
        storageContainers: []
      }
      routes: []
      fallbackRoute: {
        name: '$fallback'
        source: 'DeviceMessages'
        condition: 'true'
        endpointNames: [
          'events'
        ]
        isEnabled: true
      }
    }
    storageEndpoints: {
      '$default': {
        sasTtlAsIso8601: 'PT1H'
        connectionString: IotHubs_iot_codefirsttwins_0xacc5_demo_connectionString
        containerName: IotHubs_iot_codefirsttwins_0xacc5_demo_containerName
      }
    }
    messagingEndpoints: {
      fileNotifications: {
        lockDurationAsIso8601: 'PT1M'
        ttlAsIso8601: 'PT1H'
        maxDeliveryCount: 10
      }
    }
    enableFileUploadNotifications: false
    cloudToDevice: {
      maxDeliveryCount: 10
      defaultTtlAsIso8601: 'PT1H'
      feedback: {
        lockDurationAsIso8601: 'PT1M'
        ttlAsIso8601: 'PT1H'
        maxDeliveryCount: 10
      }
    }
    features: 'None'
  }
}

resource digitalTwinsInstances_dt_codefirsttwins_0xacc5_demo_name_resource 'Microsoft.DigitalTwins/digitalTwinsInstances@2020-12-01' = {
  name: digitalTwinsInstances_dt_codefirsttwins_0xacc5_demo_name
  location: 'australiaeast'
  tags: {
    ApplicationName: 'codefirsttwins'
    WorkloadName: 'codefirsttwins'
    DataClassification: 'Non-business'
    Criticality: 'Low'
    BusinessUnit: 'Demo'
    Env: 'demo'
  }
  properties: {
    privateEndpointConnections: []
    publicNetworkAccess: 'Enabled'
  }
}