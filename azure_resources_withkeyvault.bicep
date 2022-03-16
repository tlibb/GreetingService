
var kvResourceGroup = 'bicepstoragetinedev'
var kvName = 'tinetestdevkv'
param location string = 'northeurope'

param tenantId string




resource kv 'Microsoft.KeyVault/vaults@2019-09-01' existing = {
  name: kvName
  scope: resourceGroup(tenantId, kvResourceGroup )
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
