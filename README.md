# Benjamin Moore Retail POS Services

## Links
[Benjamin Moore Swagger hub v1.5](https://app.swaggerhub.com/apis/Benjamin-Moore/retailer-pos/1.5)

## APIM
Azure API Management is being used to direct traffic to resources within azure so that Benjamin Moore can have a single API surface for developers work with when developing new solutions.

- APIM is created as an internally facing API, this requires DNS entries to be set by Benjamin Moore, as well as an NSG.
- Integrated with Application Insights
- Integrated with azure vnet peered for communication with on-premises resources.
- Enabled for Microsoft.Web communication channels only
- MSI Enabled for communication with downstream resources.

### CustomerLoyalty
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
    - GET, PUT, POST verbs
    - Enabled for MSI

## Key Vault
Hana communication is facilitated with the use of Mutual TLS so that, with the use of a properly created certificate can securely communicate with SAP using HANA XSJS secured endpoints.

## Vnet
All vnets must be peered to the VSTS_VNET_96_0 which has the gateway subnet so that communication can reach on-premises resources.
