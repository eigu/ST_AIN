using System;

[Flags]
public enum WasteType
{
    Any = 0,
    MunicipalSolidWaste = 1 << 0,
    IndustrialWaste = 1 << 1,
    CommercialWaste = 1 << 2 ,
    ConstructionAndDemolitionDebris = 1 << 3,
    HazardousWaste = 1 << 4,
    ElectronicWaste = 1 << 5,
    AgriculturalWaste = 1 << 6
}