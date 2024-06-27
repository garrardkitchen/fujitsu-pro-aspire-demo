param location string = resourceGroup().location
param saName string
param principalId string
param principalType string

resource sa 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  location: location
  sku: {
    name: 'Standard_RAGRS'
  }
  kind: 'StorageV2'
  name: saName
  properties: {
    accessTier: 'Hot'
    dnsEndpointType: 'Standard'
    defaultToOAuthAuthentication: true
    publicNetworkAccess: 'Enabled'
    allowCrossTenantReplication: false
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    allowSharedKeyAccess: true
    largeFileSharesState: 'Enabled'
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      requireInfrastructureEncryption: false
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
        table: {
          keyType: 'Account'
          enabled: true
        }
        queue: {
          keyType: 'Account'
          enabled: true
        }       
      }
    }
  }
}

// is created automatically when published so only needed for unner-loop
resource storageTableDataContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = { // = if (principalType == 'User') {
  name: guid(sa.id, 'ba92f5b4-2d11-453d-a403-e96b0029c9fe', principalId) 
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'ba92f5b4-2d11-453d-a403-e96b0029c9fe')
    principalId: principalId
    principalType: principalType
  }
}

// output PrincipalId string = (principalType == 'User') ? storageTableDataContributorRoleAssignment.properties.principalId : ''
// output Name string = (principalType == 'User') ? storageTableDataContributorRoleAssignment.name : ''

output PrincipalId string = storageTableDataContributorRoleAssignment.properties.principalId
output Name string = storageTableDataContributorRoleAssignment.name
output Endpoint string = sa.properties.primaryEndpoints.blob
