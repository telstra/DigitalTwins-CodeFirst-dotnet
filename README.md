# Introduction 
This project intends to make a better way for developers to interact with Azure Digital Twins(ADT). Currently in ADT, digital twins and models are defined in a JSON-like language called Digital Twins Definition Language (DTDL) and they describe twins in terms of their state properties, telemetry events, commands, components, and relationships.

Writing this in DTDL provides the most flexibility but it also requires developers to do more. Developers need to learn DTDL and write the code necessary in their solutions to read and write data in DTDL for both twin models and twins. 

Telstra has developed a “code-first” approach which sees it leverage common developer Plain Old Class Objects (POCO) together with Azure Digital Twins and a new Digital Twins library that may be preferred to working with DTDL directly. This approach enables developers to define a digital twin in code and then use it to create the twin models in Azure Digital Twins. Creating the digital twins can be done in Azure DevOps using a pipeline step (provided by Telstra) that automatically converts the code first models to the necessary format and saves them to Azure Digital Twins.  

To date this has drastically reduced the time it takes for developers to create, read, write, and deploy digital twins, as well as, reducing the ongoing maintenance required. 

# Usage

## Creating twins models as classes 
Twin models can be represented are POCO classes by deriving from the `TwinBase` class along with a `DigitalTwin` attribute. 
The model contents are the properties of the class and the schema is determined by the property's attribute. Any additional data can be filled in with the attribute constructor. An example is shown below. 
```
[DigitalTwin(DisplayName = "Sample Twin")]
public class SampleTwin : TwinBase
{
    [TwinProperty("testProperty")]
    public string Property { get; set; }

    [TwinTelemetry]
    public bool Flag { get; set; }

    [TwinComponent]
    public SimpleTwin ComponentTwin { get; set; }

    [TwinRelationship(MaxMultiplicity = 1, MinMultiplicity = 2)]
    public TwinWithNestedObject TwinRelationship { get; set; }

    [TwinProperty]
    public List<int> IntArray { get; set; }

    [TwinProperty]
    public Dictionary<string, string> StringMap { get; set; }

}
```

The class can then be initialised as an object and used as a digital twin

## Serialisation and Deserialisation to DTDL
In order to use serialisation the ModelLibrary class must be initialised and with it a DigitalTwinSerialiser object must be created as shown below
```
var modelLibrary = new ModelLibrary();
var Serializer = new DigitalTwinSerializer(modelLibrary);
```

### Models
Model serialisation is done with the following two functions
```
Serializer.SerializeModel<T>()
Serializer.SerializeModel(Type t)
```
Both functions return the DTDL as a string. The first function takes the class type as a generic and the second takes the class type as a parameter of the `Type` class.

### Twins
Twin serialisation can be done with the following functions
```
Serializer.SerializeTwin(object twin)
Serializer.SerializeTwin<T>(T twin)
```
Both functions return the DTDL as a string however the function with the generic is a little faster.

# Contribution Guidelines
Telstra appreciates any contributions to the project. Please follow the [code of conduct](CODE-OF-CONDUCT.md)

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

The specifications for Digital Twins Definition Language version 2 (DTDLv2), are available 
from Microsoft under a Creative Commons (Attribution) licence at:
https://github.com/Azure/opendigitaltwins-dtdl

A diagram of the structure of DTDLv2 is provided in the docs folder [docs/pics/DTDLv2-structure.png](docs/pics/DTDLv2-structure.png)
