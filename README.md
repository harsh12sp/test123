# Benjamin Moore Retail POS Services - Create Customer Loyalty Endpoint 

# Table Of Contents<!-- omit in toc -->

- [Project Overview](#project-overview)
  - [Overview](#project-overview) 
  - [APIM](#apim)
    - [Customer Loyalty](#customer-loyalty)
        - [POST Route](#post-route)
  - [Azure Functions](#azure-functions)
  - [Key Vault](#key-vault)
  - [Vnet](#vnet)
- [Prerequisites](#prerequisites)
  - [CLI Overview and Command Reference](#cli-overview-and-command-reference)
  - [Installing Azure CLI](#installing-azure-cli)
  - [Azure CLI Login](#azure-cli-login)
- [Configuration](#configuration)
  - [Fetch Application Configuration Settings for Local Development](#fetch-application-configuration-settings-for-local-development)
  - [Decrypt the settings](#decrypt-the-settings)
- [Getting Started](#getting-started)
- [Folder Structure](#folder-structure)
- [API Documentation](#api-documentation)
- [Release Pipeline (retail-pos-v1-customers-deploy)](#release-pipeline-retail-pos-v1-customers-deploy)
  - [Variable Groups](#variable-groups)
  - [Tasks](#tasks)
  - [Release Time Variables](#release-time-variables)
  - [Environment Specific Variables](#environment-specific-variables)
  - [Parameters](#parameters)
- [Troubleshooting](#troubleshooting)
- [Links](#links)

<br />

# Project Overview
This repository contains the backend API for the Retail POS Customer Loyalty Endpoind. It's based on Azure functions. This POST API endpoint register a customer to loyalty program in Hana System. It performs the following actions

    - Customer makes a POST Call using APIM
    - Fetch Mutual TLS Hana Client API Certificate
    - Call Hana API endpoint
    - Push Mesage to Storage Queue using Event Grid (Storage Queue trigger Customer Loyalty Processor function)
    - Send Loyalty Indicator Response back to Customer

For more information about Retailer POS Architecture click [here](https://benjaminmooreappdev.atlassian.net/wiki/x/kYG0L).

## APIM
Azure API Management is being used to direct traffic to resources within azure so that Benjamin Moore can have a single API surface for developers work with when developing new solutions.

- APIM is created as an internally facing API, this requires DNS entries to be set by Benjamin Moore, as well as an NSG.
- Integrated with Application Insights
- Integrated with azure vnet peered for communication with on-premises resources.
- Enabled for Microsoft.Web communication channels only
- MSI Enabled for communication with downstream resources.

### Customer Loyalty
The route defined in apim is {apim-uri}/retailers/pos/customerloyalty.  A successful POST will return a response with both the customer number and a Y/N loyalty indicator.

#### POST Route
Creates a customer and returns a response with a customer id and a loyalty indicator (Y/N).
{apim-uri}/retailers/pos/customerloyalty?version={version}
- The string parameter **version** is required, version numbers are matched as v[1-9]\\.[0=9].
   - When version numbers are not matched an "otherwise will be selected, and may not be compatible with provided arguments.
   - Consumers will not be notified of version matching results.
- A json customer loyalty body must be provided
- On a successful creation this route will raise an event to an Azure Event Grid for any post processing.

## Azure Functions
Azure functions are using a preview mode option for the Elastic Premium plan to allow for a premium consumption model. [Function Premium Elastic Plans](https://azure.microsoft.com/en-us/blog/announcing-the-azure-functions-premium-plan-for-enterprise-serverless-workloads/).  This allows for  pre-warmed instances to spin up as well as support vnet integration with azure functions.
- Customers Azure function
    - http trigger
    - POST verb
    - Enabled for MSI

## Key Vault
Hana communication is facilitated with the use of Mutual TLS so that, with the use of a properly created certificate can securely communicate with SAP using HANA XSJS secured endpoints.

## Vnet
All vnets must be peered to the VSTS_VNET_96_0 which has the gateway subnet so that communication can reach on-premises resources.

<br />

# Prerequisites

## CLI Overview and Command Reference

The Azure Command-Line Interface (CLI) is a cross-platform command-line tool to connect to Azure and execute administrative commands on Azure resources. It allows the execution of commands through a terminal using interactive command-line prompts or a script.

## Installing Azure CLI

The current version of the Azure CLI is 2.40.0. For information about the latest release, see the release notes. To find your installed version and see if you need to update it, run the az version.

- [Install on MacOS](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-macos)
- [Install on Windows](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli)

After installing the Azure CLI. Make sure you have successfully installed Azure CLI. Using the command
```sh
az version
```

> Note: You will get errors in case an invalid subscription or function name is provided in the CLI commands. Check the Troubleshooting section for the same.

## Azure CLI Login

Before logging in, make sure you have Azure subscription access. If not, please request from your administrator, otherwise, login will fail.

Now open the command line or PowerShell on your machine and log into your Azure Account by running the following command. 
```sh
az login
```
If you work with multiple Azure subscriptions to set the subscription that your Function is hosted in, run this command: 

```sh
az account set -s "[subscription-name-or-id]"
```  

<br />

# Configuration

## Fetch Application Configuration Settings for Local Development

Now you can fetch the app settings from your Function App in Azure. Make sure you are in your project's source/[function-name].FunctionApp directory and run the following command in your terminal:

```sh
func azure functionapp fetch-app-settings '[function-name]'
```

Go back into your project in Editor and check out your local.settings.json file. You can see that your Function settings have been retrieved and written to your local settings file!

Our settings are encrypted! That's not going to help us when running our Function locally! Function app settings are encrypted when stored and are only decrypted before being injected into your Function's process memory when it starts.

## Decrypt the settings

Decrypt the settings. In your terminal, run the following command.

```sh
func settings decrypt
```

For more information click this [link](https://benjaminmooreappdev.atlassian.net/l/cp/ytXLS7wX).   

<br />

# Getting Started

If you're running the functions on local. Make sure to update the database to **Sandbox** in the **local.settings.json** created through the above configuration section.

To run the Azure functions you need to get into the source/[function-name].FunctionApp directory and run the below commands

```sh
func start
```
OR
```sh
dotnet run
```

This will start the Azure functions and give the below output
```sh
Azure Functions Core Tools
Core Tools Version:       4.0.5030 Commit hash: N/A  (64-bit)
Function Runtime Version: 4.15.2.20177

[2024-01-11T07:21:02.745Z] Found ${WORKSPACE}\\retail-pos-v1-customers\\source\\Customers.FunctionApp\\BenjaminMoore.Api.Retail.Pos.Customers.FunctionApp.csproj. Using for user secrets file configuration.

Functions:

        CreateCustomerLoyalty: [POST] http://localhost:7071/retailers/pos/customerloyalty

For detailed output, run func with --verbose flag.

```   

<br />

# Folder Structure

For each endpoint we create a new directory and give a host.json file which contains the API configs. Some frequently used files in the project:-
| File           | Path                                                     | Description                                                                                                         |
| -------------- | -------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| Constants file | source\\[function-app].FunctionApp\Utils\constants.cs    | This file contains the constants that are used throughout the application                                           |
| Error Handler  | source\\[function-app].FunctionApp\Utils\errorHandler.cs | This is the fallback error handler which is called when any error is thrown in the application/service layer        |
| Logger         | source\\[function-app].FunctionApp\Utils\logger.cs       | This is the logger module which gets the config and constant values and returns a logger object from bmc-common-lib |


<br />

# API Documentation

You can download the Postman collection from [here](docs\collection.json) and import it into Postman.  

<br />

# Release Pipeline (retail-pos-v1-customer-loyalty-endpoints-deploy)
The release pipeline will create/update an azure function, and deploy the codebase out to the azure function.  The function will be integrated with the vnet, MSI will be enabled for the function.  Key vault permissions will be granted for the MSI identity of the function.

## Variable Groups
[retail-pos-common] variable group is referenced for shared variable names across all pipelines.

## Tasks
- Deploy Azure Function  
Deploys an azure function out to azure using the function.json template.
- Azure Function App Deploy: $(appName)  
Deploys the code from the repository out to the azure function.
- Key Vault Access  
Sets up key vault access for the azure function to be able to read secrets and certificates from azure key vault.

## Release Time Variables
There are no release time variables defined

## Environment Specific Variables
- env defines the name of the environment and is already predefined as dev, qa, uat, or prd

## Parameters
- location - Currently must be eastus.  If the region changes for vnet peering all the region will need to change for all resources.
- appName - The name of the function app. 
- appServicePlanName - The name of the app service plan name that the function will use.
- storageAccountName - Required storage account name for the function app.  The connection string will be returned to the ARM template, assumes the storage account resides in the same resource group.
- appInsightsName - the name of the app insights instance.  Assumes the app insights instance is in the same resource group. 
- keyVaultBaseUri - The base uri (can be found in the overview section of the key vault). 
- vnetName - The name of the key vault resource.  Assumes that the vnet is in the resource group. 
- functionsSubnetName - The name of the subnet that the function will use for vnet integration.
- mutualTlsCertificateName - The name of the certificate name for mutual tls auth.   

<br />

# Troubleshooting
- Missing or unreadable key in key vault  
When the webapiuser key is missing, or otherwise unreadable in the keyvault the function will return a 500 error.  It is detectable in AI by a KeyNotFound exception.  
Solution: Check the key vault exists, that the key exists, and that there are read permissions for the certificates [get, list, get certificate authorities, list certificate authorities], and for secrets must be able to [get, list].
- Socket Errors Reading from Hana
When the function cannot access on premises resources such as hana a 500 error will be returned by the function.  This condition is detectable by a SocketException with details about the Hana endpoint.
Solution: Ensure that vnet peering is configured, and that the proper configuration is on the firewall on premise for the vnet CIDR block.  Additional troubleshooting can be done by creating a virtual machine in the same vnet that is having trouble and pursuing standard network troubleshooting.
- Changed deployment does not appear to have updated.
When a function is deployed by some locking has occurred the function will not be properly updated, but to all appearances the pipeline has succeeded.
Solution:  Use the kudu console of the function to investigate assemblies that may not have been updated and compare them against the files in the deployed artifact.  The individual deployment can be re-released by going to the specific release instance, selecting the stage of deployment (DEV, QA, UAT, PROD), and selecting Deploy.   

<br />

## Links
 - [Swagger Hub Retailer POS](https://app.swaggerhub.com/apis/Benjamin-Moore/Retailer-POS/v1)
 - [Swagger Hub Retailer POS API Services](https://app.swaggerhub.com/apis/Benjamin-Moore/retailer-pos/1.7)
 - [Swagger Hub POS Transactions](https://app.swaggerhub.com/apis/Benjamin-Moore/Transaction/v4)
