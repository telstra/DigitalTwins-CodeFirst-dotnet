Code First Azure Digital Twins -- End to end example
====================================================

Requirements:

* dotnet (3.1 LTS or higher)
* PowerShell (7.0 LTS or higher)

Based on https://github.com/Azure-Samples/digital-twins-samples/tree/master/HandsOnLab


```pwsh
az login
az account set --subscription "<your-Azure-subscription-ID>"
```


```
az group create --location <region> --name <name-for-your-resource-group>
az dt create --dt-name <name-for-your-Azure-Digital-Twins-instance> --resource-group <your-resource-group> --location <region>
az dt show --dt-name <your-Azure-Digital-Twins-instance>.

az iot hub create --name <name-for-your-IoT-hub> --resource-group <your-resource-group> --sku S1
```



```
Set-AzContext -SubscriptionId 00000000-0000-0000-0000-000000000000
Register-AzResourceProvider -ProviderNamespace Microsoft.DigitalTwins
Install-Module -Name Az.DigitalTwins
New-AzResourceGroup -Name <name-for-your-resource-group> -Location <region>
New-AzDigitalTwinsInstance -ResourceGroupName <your-resource-group> -ResourceName <name-for-your-Azure-Digital-Twins-instance> -Location <region>
Get-AzDigitalTwinsInstance -ResourceGroupName <your-resource-group> -ResourceName <name-for-your-Azure-Digital-Twins-instance> |
  Select-Object -Property *

