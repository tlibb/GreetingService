
var kvResourceGroup = 'bicepstoragetinedev'
var kvName = 'tinetestdevkv'
param location string = 'westeurope'

param tenantId string




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
