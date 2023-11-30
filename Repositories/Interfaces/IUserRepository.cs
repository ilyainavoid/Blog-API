using BlogApi.Models.Entities;

namespace BlogApi.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByEmail(string email);
}