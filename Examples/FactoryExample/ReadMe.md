Code First Azure Digital Twins -- End to end example
====================================================

Requirements:

* dotnet (3.1 LTS or higher)
* PowerShell (7.0 LTS or higher)

Based on https://github.com/Azure-Samples/digital-twins-samples/tree/master/HandsOnLab

Create infrastructure
---------------------

You need to create a digital twins instance in Azure to deploy the model to. There is a
script `deploy-infrastructure.ps1` that will created the needed resources.

To run the script you need to load the required PowerShell modules, then connect to your
Azure account and set the context for the subscription you want to use, then run the
script.

``` pwsh
  Install-Module -Name Az -Scope CurrentUser -Force
  Install-Module -Name Az.DigitalTwins -Scope CurrentUser -Force
  Register-AzResourceProvider -ProviderNamespace Microsoft.DigitalTwins
  
  Connect-AzAccount
  Set-AzContext -SubscriptionId $SubscriptionId
  
  $VerbosePreference = 'Continue'
  ./deploy-infrastructure.ps1
```

Digital Twins Explorer
----------------------

After you have created the infrastructure, open `https://portal.azure.com/` and go to the resource group.

Open the Azure Digital Twins resource.

Copy the Host name, and then open the Azure Digital Twins Explorer (preview).

For the Azure Digital Twins URL, use `https://<digital twins host name>`, with the Host name copied from above.

The explorer should start out empty (see below).

Running basic checks
--------------------

To see the example serialized model:

```
dotnet run -- --serialize model
```

To see the example serialized twin instances:

```
dotnet run -- --serialize twin
```

To run parse and validate the model with `Microsoft.Azure.DigitalTwins.Parser`: 

```
dotnet run -- --parse model
```

Running the example to create models and twins
----------------------------------------------

To upload the models to Azure:

``` pwsh
$rgName = "rg-codefirsttwins-dev-001"
$dtName = "dt-codefirsttwins-0x$((Get-AzContext).Subscription.Id.Substring(0,4))-dev"
$hostName = (Get-AzDigitalTwinsInstance -ResourceGroupName $rgName -ResourceName $dtName).HostName
dotnet run -- --create model --endpoint "https://$hostName"
```

After this, if you refresh the Models in Digital Twins Explorer, you will see the uploaded models. 

You then need to upload the Twin instances:

``` pwsh
dotnet run -- --create twin --endpoint "https://$hostName"
```

To see the Twin instances, ensure the query explorer has the default query `SELECT * FROM digitaltwins`,
and then click Run Query. The twin instances should appear in the Twins list and Twin Graph.

Cleanup
-------

After running the example, you can clean up the Azure resources (to save money).

``` pwsh
./remove-infrastructure.ps1
```


