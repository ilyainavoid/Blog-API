using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BlogApi.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GarAddressLevel
{
    [EnumMember(Value = "Region")]
    Region,
    
    [EnumMember(Value = "AdministrativeArea")]
    AdministrativeArea,
    
    [EnumMember(Value = "MunicipalArea")]
    MunicipalArea,
    
    [EnumMember(Value = "RuralUrbanSettlement")]
    RuralUrbanSettlement,
    
    [EnumMember(Value = "City")]
    City,
    
    [EnumMember(Value = "Locality")]
    Locality,
    
    [EnumMember(Value = "ElementOfPlanningStructure")]
    ElementOfPlanningStructure,
    
    [EnumMember(Value = "ElementOfRoadNetwork")]
    ElementOfRoadNetwork,
    
    [EnumMember(Value = "Land")]
    Land,
    
    [EnumMember(Value = "Building")]
    Building,
    
    [EnumMember(Value = "Room")]
    Room,
    
    [EnumMember(Value = "RoomInRooms")]
    RoomInRooms,
    
    [EnumMember(Value = "AutonomousRegionLevel")]
    AutonomousRegionLevel,
    
    [EnumMember(Value = "IntracityLevel")]
    IntracityLevel,
    
    [EnumMember(Value = "AdditionalTerritoriesLevel")]
    AdditionalTerritoriesLevel,
    
    [EnumMember(Value = "LevelOfObjectsInAdditionalTerritories")]
    LevelOfObjectsInAdditionalTerritories,
    
    [EnumMember(Value = "CarPlace")]
    CarPlace
}