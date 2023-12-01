using BlogApi.Models.DTO;

namespace BlogApi.Services;

public interface IUserService
{
    Task<TokenResponse> Register(UserRegisterModel userRegisterModel);
    Task<TokenResponse> Login(LoginCredentials credentials);
    Task Logout(string token);
    Task<UserDto> GetProfileInfo(Guid id);
    Task EditProfileInfo(Guid id, UserEditModel userEditModel);
}