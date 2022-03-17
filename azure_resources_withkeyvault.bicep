
var kvResourceGroup = 'bicepstoragetinedev'
var kvName = 'tinetestdevkv'
param location string = 'westeurope'

param tenantId string




resource kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: kvName
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
        objectId: '28c3384b-4185-44bf-b439-afd517209d04'
        tenantId: tenantId
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
