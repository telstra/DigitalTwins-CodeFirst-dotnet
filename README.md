# Introduction 
This project intends to make a better way for developers to interact with Azure Digital Twins(ADT). Currently in ADT, digital twins and models are defined in a JSON-like language called Digital Twins Definition Language (DTDL) and they describe twins in terms of their state properties, telemetry events, commands, components, and relationships.

Writing this in DTDL provides the most flexibility but it also requires developers to do more. Developers need to learn DTDL and write the code necessary in their solutions to read and write data in DTDL for both twin models and twins. 

Telstra has developed a “code-first” approach which sees it leverage common developer Plain Old Class Objects (POCO) together with Azure Digital Twins and a new Digital Twins library that may be preferred to working with DTDL directly. This approach enables developers to define a digital twin in code and then use it to create the twin models in Azure Digital Twins. Creating the digital twins can be done in Azure DevOps using a pipeline step (provided by Telstra) that automatically converts the code first models to the necessary format and saves them to Azure Digital Twins.  

To date this has drastically reduced the time it takes for developers to create, read, write, and deploy digital twins, as well as, reducing the ongoing maintenance required. 

# Getting Started
Ensure that your system has .NET core 3.1

# Setup
## Environment Setup
- Create ADT instance
- Create IoT Hub instance 
- Integrate IoT Hub instance with ADT instance
## Configuring Unit Tests
Ensure that the following environment variables are set. Optionally, they can also be in the configuration.
- TENANT_ID
- CLIENT_ID
- CLIENT_SECRET
- SERVICE_HOSTNAME

# Build and Test
The project is meant to be used as a library so all testing has do be done through unit testing with the xUnit framework.

# Contribution Guidelines
Telstra appreciates any contributions to the project.

## Issues
Feel free to submit feature requests and bugs as issues. 

## Contributing
The project follows a Feature Branch Workflow. So for each feature or bug a new branch must be created and all changes must be done on that branch. All commits must have a verified signature. 

1. Clone the project on your local machine
2. Create a new feature/bug branch from master
3. Push the changes
4. Submit a pull request

## Testing
Ensure that all new features added are tested and have unit tests for the same in the xUnit framework.
# References
This project is based on the Microsoft SDK recommendations outlined at 
https://azure.github.io/azure-sdk/dotnet_introduction.html.
