using System.Diagnostics.Metrics;
using BlogApi.Data;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services.Address;

public class AddressService : IAddressService
{
    private readonly GarDbContext _contextGar;

    public AddressService(GarDbContext context)
    {
        _contextGar = context;
    }
    
    public async Task<List<SearchAddressModel>> Search(long? parentObject, string? query)
    {
        parentObject ??= 0;
        query ??= string.Empty;
        var resultHierarchyObjects = await _contextGar.AsAdmHierarchies.Where(obj => obj.Parentobjid == parentObject).ToListAsync();

        var result = new List<SearchAddressModel>();
        foreach (var hierarchyObject in resultHierarchyObjects)
        {
            var house = await _contextGar.AsHouses.FirstOrDefaultAsync(obj => obj.Objectid == hierarchyObject.Objectid);
            var addressObject = await _contextGar.AsAddrObjs.FirstOrDefaultAsync(obj => obj.Objectid == hierarchyObject.Objectid);
            
            if (addressObject != null)
            {
                if (query != null && addressObject.Name.ToLower().Contains(query.ToLower()))
                {
                    int level = 0;
                    if (int.TryParse(addressObject.Level, out int levelValue))
                    {
                        level = levelValue;
                    }
                    var levelText = AddressLevelText.GetData((GarAddressLevel)level-1);
                    var addressObjectDto = new SearchAddressModel
                    {
                        ObjectId = addressObject.Objectid,
                        ObjectGuid = addressObject.Objectguid,
                        Text = $"{addressObject.Typename} {addressObject.Name}",
                        ObjectLevel = (GarAddressLevel)level - 1,
                        ObjectLevelText = levelText
                    };
                    result.Add(addressObjectDto);   
                }
            }
            else if (house != null)
            {
                if (query != null && house.Housenum.ToLower().Contains(query.ToLower()))
                {
                    int level = 10;
                    var levelText = AddressLevelText.GetData((GarAddressLevel)level-1);
                    var addressObjectDto = new SearchAddressModel
                    {
                        ObjectId = house.Objectid,
                        ObjectGuid = house.Objectguid,
                        Text = house.Housenum,
                        ObjectLevel = (GarAddressLevel)level - 1,
                        ObjectLevelText = levelText
                    };
                    result.Add(addressObjectDto); 
                }
            }
        }

        return result;
    }

    public async Task<List<SearchAddressModel>> GetChain(Guid objectGuid)
    {
        //Get Id of the last chain element
        long rootId;
        var house = await _contextGar.AsHouses.FirstOrDefaultAsync(h => h.Objectguid == objectGuid);
        var addressObj = await _contextGar.AsAddrObjs.FirstOrDefaultAsync(obj => obj.Objectguid == objectGuid);
        if (addressObj != null)
        {
            rootId = addressObj.Objectid;
        }
        else if (house != null)
        {
            rootId = house.Objectid;
        }
        else
        {
            throw new Exception($"Not Found found object with ObjectGuid={objectGuid}");
        }


        //Get address chain
        var hierarchyObject = await _contextGar.AsAdmHierarchies.FirstOrDefaultAsync(obj => obj.Objectid == rootId);
        string path = hierarchyObject.Path;
        var pathIds = hierarchyObject.Path.Split('.');
        
        //Creating result list
        var result = new List<SearchAddressModel>();
        for (int i = 0; i < pathIds.Length; i++)
        {
            var addressObject = await _contextGar.AsAddrObjs.FirstOrDefaultAsync(obj => obj.Objectid.ToString() == pathIds[i]);
            var houseObject = await _contextGar.AsHouses.FirstOrDefaultAsync(obj => obj.Objectid.ToString() == pathIds[i]);

            //AddressObject is a house
            if (addressObject == null)
            {
                int level = 10;
                var levelText = AddressLevelText.GetData((GarAddressLevel)level-1);
                var addressObjectDto = new SearchAddressModel
                {
                    ObjectId = houseObject.Objectid,
                    ObjectGuid = houseObject.Objectguid,
                    Text = houseObject.Housenum,
                    ObjectLevel = (GarAddressLevel)level - 1,
                    ObjectLevelText = levelText
                };
                result.Add(addressObjectDto);   
            }
            //AddressObject is another type of address object
            else
            {
                int level = 10;
                if (int.TryParse(addressObject.Level, out int levelValue))
                {
                    level = levelValue;
                }
                var levelText = AddressLevelText.GetData((GarAddressLevel)level-1);
                var addressObjectDto = new SearchAddressModel
                {
                    ObjectId = addressObject.Objectid,
                    ObjectGuid = addressObject.Objectguid,
                    Text = $"{addressObject.Typename} {addressObject.Name}",
                    ObjectLevel = (GarAddressLevel)level - 1,
                    ObjectLevelText = levelText
                };
                result.Add(addressObjectDto);   
            }
        }

        return result;
    }
}