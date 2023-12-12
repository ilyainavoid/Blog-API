using BlogApi.Models.Enums;

namespace BlogApi.Data;

public static class AddressLevelText
{
    private static Dictionary<GarAddressLevel, string> LevelTextValues = new Dictionary<GarAddressLevel, string>
    {
        { GarAddressLevel.Region, "Субъект РФ" },
        { GarAddressLevel.AdministrativeArea, "Административный район" },
        { GarAddressLevel.MunicipalArea, "Муниципальный район" },
        { GarAddressLevel.RuralUrbanSettlement, "Сельское/городское поселение" },
        { GarAddressLevel.City, "Город" },
        { GarAddressLevel.Locality, "Населенный пункт" },
        { GarAddressLevel.ElementOfPlanningStructure, "Элемент планировочной структуры" },
        { GarAddressLevel.ElementOfRoadNetwork, "Элемент улично-дорожной сети" },
        { GarAddressLevel.Land, "Земельный участок" },
        { GarAddressLevel.Building, "Здание (сооружение)" },
        { GarAddressLevel.Room, "Помещение" },
        { GarAddressLevel.RoomInRooms, "Помещения в пределах помещения" },
        { GarAddressLevel.AutonomousRegionLevel, "Уровень автономного округа" },
        { GarAddressLevel.IntracityLevel, "Уровень внутригородской территории" },
        { GarAddressLevel.AdditionalTerritoriesLevel, "Уровень дополнительных территорий" },
        { GarAddressLevel.LevelOfObjectsInAdditionalTerritories, "Уровень объектов на дополнительных территориях" },
        { GarAddressLevel.CarPlace, "Машиноместо" },
    };

    public static string? GetData(GarAddressLevel level)
    {
        if (LevelTextValues.TryGetValue(level, out var value))
        {
            return value;
        }
        return null;
    }
}