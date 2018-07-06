# A simple web app to demo AAD Pod Identity

https://github.com/Azure/aad-pod-identity

## How to deploy

Set up AAD Pod Identity (instructions in repo above), and create an Azure Identity (MSI)

### Edit Deployment File ``kubectl apply -f msi/srcid/tester.deploy.yaml``

Make sure the label `aadpodidbinding` matches your identity binding.

```yaml
      labels:
        app: tester
        aadpodidbinding: tester
```

### Deploy 

Deploy a service to access the web page:

`kubectl apply -f msi/srcid/tester.service.yaml`

Deploy the tester app:

`kubectl apply -f msi/srcid/tester.deploy.yaml`

## TODO

Make page for testing each of the following:

- Azure Resource Manager 	https://management.azure.com/ 	
- Azure Key Vault 	https://vault.azure.net 	
- Azure Data Lake 	https://datalake.azure.net/ 	
- Azure SQL 	https://database.windows.net/ 	
- Azure Event Hubs 	https://eventhubs.azure.net 	
- Azure Service Bus 	https://servicebus.azure.net 	
- Azure Storage 	https://storage.azure.com/ 