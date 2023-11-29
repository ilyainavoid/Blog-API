using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Repositories;
using Microsoft.Win32.SafeHandles;

namespace BlogApi.Services;

public class UserService : IUserService
{
    private readonly TokenUtilities _tokenUtility;
    private readonly UserRepository _userRepository;

    public UserService(TokenUtilities tokenUtility,UserRepository repository)
    {
        _tokenUtility = tokenUtility;
        _userRepository = repository;
    }
    
    
    public Task<TokenResponse> Register(UserRegisterModel userRegisterModel)
    {
        //throw new NotImplementedException();
    }

    public async Task<TokenResponse> Login(LoginCredentials credentials)
    {
        var user = await _userRepository.GetUserByEmail(credentials.Email);
        if (user == null)
        {
            throw new Exception();
        }

        bool isValidPassword = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);

        if (!isValidPassword)
        {
            throw new Exception();
        }

        var token = _tokenUtility.GenerateToken(user);
        return new TokenResponse(token);
    }

    public Task<Response> Logout(string token)
    {
        //throw new NotImplementedException();
    }

    public Task<UserDto> GetProfileInfo(Guid id)
    {
        //throw new NotImplementedException();
    }

    public Task EditProfileInfo(Guid id, UserEditModel userEditModel)
    {
        //throw new NotImplementedException();
    }
}