using BlogApi.Models.DTO;

namespace BlogApi.Services.Address;

public interface IAddressService
{
    Task<List<SearchAddressModel>> Search(long? parentObject, string? query);
    Task<List<SearchAddressModel>> GetChain(Guid objectGuid);
}