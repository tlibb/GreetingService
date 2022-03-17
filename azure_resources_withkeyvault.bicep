

param location string = 'westeurope'

param tenantId string


@secure()
param sqlAdminUser string
@secure()
param sqlAdminPassword string
@secure()
param ServiceBusConnectionKey string
@secure()
param WebhookUrl string




resource kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: 'tinetestdevkv'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenantId

    enableRbacAuthorization: false      // Using Access Policies model
    accessPolicies: [
      {
        tenantId: tenantId
        objectId: '73c52ba9-45be-42ab-ad33-617ef3ff1515'
        permissions: {
          secrets: [
            'all'
          ]
          certificates: [
            'all'
          ]
          keys: [
            'all'
          ]
        }
      }
    ]

    enabledForDeployment: true          // VMs can retrieve certificates
    enabledForTemplateDeployment: true  // ARM can retrieve values

    enablePurgeProtection: true         // Not allowing to purge key vault or its objects after deletion
    enableSoftDelete: true
    softDeleteRetentionInDays: 90    
    createMode: 'default'       
  }    
}

resource secret1 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: 'ServerAdminLogin'
  parent: kv  // Pass key vault symbolic name as parent
  properties: {
    attributes: {
      enabled: true
    }
    value: sqlAdminUser
  }
}

resource secret2 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: 'ServerAdminPassword'
  parent: kv  // Pass key vault symbolic name as parent
  properties: {
    attributes: {
      enabled: true
    }
    value: sqlAdminPassword
  }
}

resource secret3 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: 'ServiceBusConnectionString'
  parent: kv  // Pass key vault symbolic name as parent
  properties: {
    attributes: {
      enabled: true
    }
    value: ServiceBusConnectionKey
  }
}
resource secret4 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: 'WebhookUrl'
  parent: kv  // Pass key vault symbolic name as parent
  properties: {
    attributes: {
      enabled: true
    }
    value: WebhookUrl
  }
}

module deployResources './azure_resources.bicep' = {
  name: 'deployResources'
  params: {
    location: location
    sqlAdminUser: kv.getSecret('ServerAdminLogin')
    sqlAdminPassword: kv.getSecret('ServerAdminPassword')
    ServiceBusConnectionKey: kv.getSecret('ServiceBusConnectionString')
    WebhookUrl: kv.getSecret('WebhookUrl')
  }
} 
