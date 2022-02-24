param location string = 'northeurope'

// storage accounts must be between 3 and 24 characters in length and use numbers and lower-case letters only

var storageAccountName = 'bicepstoragetinedev' 
var hostingPlanName = 'BicepNorthEuropePlan'
var appInsightsName = 'BicepFunctionInsights'
var functionAppName = 'ANewGreetingService'

param sqlAdminUser string
param sqlAdminPassword string

// targetScope = 'subscription'

// resource rg 'Microsoft.Resources/resourceGroups@2021-01-01' = {
//   name: 'BicepResourceGroupTineDev'
//   location: location
// }

resource storageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
}

resource createStorage 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: 'tinesblobstorage'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: { 
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
  tags: {
    // circular dependency means we can't reference functionApp directly  /subscriptions/<subscriptionId>/resourceGroups/<rg-name>/providers/Microsoft.Web/sites/<appName>"
     'hidden-link:/subscriptions/${subscription().id}/resourceGroups/${resourceGroup().name}/providers/Microsoft.Web/sites/${functionAppName}': 'Resource'
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2020-10-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1' 
    tier: 'Dynamic'
  }
}

resource sqlserver 'Microsoft.Sql/servers@2019-06-01-preview' = {
  name: 'greeting-sql-dev-tine'
  location: 'westeurope'
  properties: {
    administratorLogin: sqlAdminUser
    administratorLoginPassword: sqlAdminPassword // DON'T DO THIS - EVER
    version: '12.0'
  }
}


resource sqldatabase 'Microsoft.Sql/servers/databases@2019-06-01-preview'={
  parent: sqlserver
  name: 'greering-sqldb-dev'
  location: 'westeurope' 
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  } 
}
 

resource functionApp 'Microsoft.Web/sites@2020-06-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    httpsOnly: true
    serverFarmId: hostingPlan.id
    clientAffinityEnabled: true
    siteConfig: {
      appSettings: [
        {
          'name': 'APPINSIGHTS_INSTRUMENTATIONKEY'
          'value': appInsights.properties.InstrumentationKey
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
          'name': 'FUNCTIONS_EXTENSION_VERSION'
          'value': '~4'
        }
        {
          'name': 'FUNCTIONS_WORKER_RUNTIME'
          'value': 'dotnet'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
          name: 'LoggingStorageAccount'
          value: 'DefaultEndpointsProtocol=https;AccountName=tinesblobstorage;AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'GreetingDbConnectionString'
          value: 'Server=tcp:${reference(sqlserver.id).fullyQualifiedDomainName},1433;Initial Catalog=${sqldatabase.name};Persist Security Info=False;User ID=${sqlAdminUser};Password=${sqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
        }
        // WEBSITE_CONTENTSHARE will also be auto-generated - https://docs.microsoft.com/en-us/azure/azure-functions/functions-app-settings#website_contentshare
        // WEBSITE_RUN_FROM_PACKAGE will be set to 1 by func azure functionapp publish
      ]
    }
  }

  
}

