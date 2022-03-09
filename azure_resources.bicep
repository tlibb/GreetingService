param location string = 'northeurope'

// storage accounts must be between 3 and 24 characters in length and use numbers and lower-case letters only

var storageAccountName = 'bicepstoragetinedev' 
var hostingPlanName = 'BicepNorthEuropePlan'
var appInsightsName = 'BicepFunctionInsights'
var functionAppName = 'ANewGreetingService'

param sqlAdminUser string
param sqlAdminPassword string
param ServiceBusConnectionKey string

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
#disable-next-line BCP073
    tier: 'Standard'
  }
}

resource createStorage 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: 'tinesblobstorage'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
#disable-next-line BCP073
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
    administratorLoginPassword: sqlAdminPassword 
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

resource servicebus 'Microsoft.ServiceBus/namespaces@2021-06-01-preview' = {
  name: 'tine-sb-dev'
  location: 'westeurope'
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource topic 'Microsoft.ServiceBus/namespaces/topics@2021-06-01-preview' = {
  name: 'main'
  parent: servicebus
  properties: {
    defaultMessageTimeToLive: 'P14D' //ISO 8601
    status: 'Active'
  }
}

resource greeting_create 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-06-01-preview' = {
  name: 'greeting_create'
  parent: topic
  properties: {
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P14D'
    lockDuration: 'PT30S'
    maxDeliveryCount: 10
    status: 'Active'
  }
}

resource NewGreeting 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2021-06-01-preview' = {
  name: 'NewGreeting'
  parent: greeting_create
  properties: {
    filterType: 'CorrelationFilter'
    correlationFilter: {
      properties: {
        'label/subect':'NewGreeting'
      }
    }
  }
}

resource greeting_update 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-06-01-preview' = {
  name: 'greeting_update'
  parent: topic
  properties: {
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P14D'
    lockDuration: 'PT30S'
    maxDeliveryCount: 10
    status: 'Active'
  }
}

resource PutGreeting 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2021-06-01-preview' = {
  name: 'PutGreeting'
  parent: greeting_update
  properties: {
    filterType: 'CorrelationFilter'
    correlationFilter: {
      properties: {
        'label/subect':'PutGreeting'
      }
    }
  }
}

resource user_create 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-06-01-preview' = {
  name: 'user_create'
  parent: topic
  properties: {
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P14D'
    lockDuration: 'PT30S'
    maxDeliveryCount: 10
    status: 'Active'
  }
}

resource PostUser 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2021-06-01-preview' = {
  name: 'PostUser'
  parent: greeting_update
  properties: {
    filterType: 'CorrelationFilter'
    correlationFilter: {
      properties: {
        'label/subect':'PostUser'
      }
    }
  }
}

resource user_update 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-06-01-preview' = {
  name: 'user_update'
  parent: topic
  properties: {
    deadLetteringOnMessageExpiration: false
    defaultMessageTimeToLive: 'P14D'
    lockDuration: 'PT30S'
    maxDeliveryCount: 10
    status: 'Active'
  }
}

resource PutUser 'Microsoft.ServiceBus/namespaces/topics/subscriptions/rules@2021-06-01-preview' = {
  name: 'PutUser'
  parent: greeting_update
  properties: {
    filterType: 'CorrelationFilter'
    correlationFilter: {
      properties: {
        'label/subect':'PutUser'
      }
    }
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
          value: 'DefaultEndpointsProtocol=https;AccountName=tinesblobstorage;AccountKey=${listKeys(createStorage.id, createStorage.apiVersion).keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'GreetingDbConnectionString'
          value: 'Server=tcp:${reference(sqlserver.id).fullyQualifiedDomainName},1433;Initial Catalog=${sqldatabase.name};Persist Security Info=False;User ID=${sqlAdminUser};Password=${sqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
        }
        {
          name: 'ServiceBusConnectionString'
          value: 'Endpoint=sb://tine-sb-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=${ServiceBusConnectionKey}'
        }
        // WEBSITE_CONTENTSHARE will also be auto-generated - https://docs.microsoft.com/en-us/azure/azure-functions/functions-app-settings#website_contentshare
        // WEBSITE_RUN_FROM_PACKAGE will be set to 1 by func azure functionapp publish
      ]
    }
  }

  
}

