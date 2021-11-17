using System;
using FactoryExample.Models;
using Telstra.Twins.Core;
using Telstra.Twins.Services;

Console.WriteLine("Digital Twin Code First Factory Example");

var modelLibrary = new ModelLibrary();
var serializer = new DigitalTwinSerializer(modelLibrary);

Console.WriteLine("Models:");
Console.WriteLine();

var model1Dtdl = serializer.SerializeModel<Factory>();
Console.WriteLine(model1Dtdl);
Console.WriteLine();

var model2Dtdl = serializer.SerializeModel<FactoryFloor>();
Console.WriteLine(model2Dtdl);
Console.WriteLine();

Console.WriteLine("Twins:");
Console.WriteLine();

var factory = new Factory
{
    FactoryId = "factory1", Country = "AU", ZipCode = "4000", FactoryName = "Chocolate Factory"
};
factory.Floors.Add(new FactoryFloor { FloorId = "floor1", FloorName = "Floor 1" });

var twin1Dtdl = serializer.SerializeTwin(factory);
Console.WriteLine(twin1Dtdl);
Console.WriteLine();

var twin2Dtdl = serializer.SerializeTwin(factory.Floors[0]);
Console.WriteLine(twin2Dtdl);
Console.WriteLine();
