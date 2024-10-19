using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waste : MonoBehaviour
{
    public string wasteName;

    public WasteMaterial wasteMaterial;

    private WasteType wasteType;
    
    
    void Start()
    {
        wasteType = GetWasteType(wasteMaterial);
    }

    public WasteType GetWasteType(WasteMaterial material)
        {
            switch (material)
            {
                // Municipal Solid Waste
                case WasteMaterial.Paper:
                case WasteMaterial.Plastic:
                case WasteMaterial.FoodScraps:
                case WasteMaterial.Textiles:
                case WasteMaterial.PackagingMaterials:
                case WasteMaterial.YardWaste:
                    return WasteType.MunicipalSolidWaste;
    
                // Industrial Waste
                case WasteMaterial.ScrapMetal:
                case WasteMaterial.Chemicals:
                case WasteMaterial.Solvents:
                case WasteMaterial.Sludge:
                    return WasteType.IndustrialWaste;
    
                // Commercial Waste

                // Construction and Demolition Debris
                case WasteMaterial.Concrete:
                case WasteMaterial.Wood:
                case WasteMaterial.Bricks:
                case WasteMaterial.Asphalt:
                    return WasteType.ConstructionAndDemolitionDebris;
    
                // Hazardous Waste
                case WasteMaterial.Batteries:
                case WasteMaterial.FluorescentBulbs:
                case WasteMaterial.Pesticides:
                    return WasteType.HazardousWaste;
    
                // Electronic Waste
                case WasteMaterial.Electronics:
                    return WasteType.ElectronicWaste;
    
                // Agricultural Waste
                case WasteMaterial.CropResidues:
                case WasteMaterial.AnimalManure:
                    return WasteType.AgriculturalWaste;
    
                default:
                    return WasteType.None; // Handle unknown materials
            }
        }
}

public enum WasteType
{
    None,
    MunicipalSolidWaste,
    IndustrialWaste,
    CommercialWaste,
    ConstructionAndDemolitionDebris,
    HazardousWaste,
    ElectronicWaste,
    AgriculturalWaste
}

public enum WasteMaterial
{
    None,
    Paper,
    Plastic,
    FoodScraps,
    Textiles,
    PackagingMaterials,
    YardWaste,
    ScrapMetal,
    Chemicals,
    Solvents,
    Sludge,
    Electronics,
    Concrete,
    Wood,
    Bricks,
    Asphalt,
    Batteries,
    FluorescentBulbs,
    Pesticides,
    CropResidues,
    AnimalManure
}
