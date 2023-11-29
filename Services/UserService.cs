using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Profiles;
using BlogApi.Repositories;
using Microsoft.Win32.SafeHandles;

namespace BlogApi.Services;

public class UserService : IUserService
{
    private readonly TokenUtilities _tokenUtility;
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(TokenUtilities tokenUtility,UserRepository repository, IMapper mapper)
    {
        _tokenUtility = tokenUtility;
        _userRepository = repository;
        _mapper = mapper;
    }
    
    
    public async Task<TokenResponse> Register(UserRegisterModel userRegisterModel)
    {
        var user = await _userRepository.GetUserByEmail(userRegisterModel.Email);
        if (user != null)
        {
            throw new Exception();
        }

        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var HashedPassword =
            BCrypt.Net.BCrypt.HashPassword(userRegisterModel.Password, salt);
        userRegisterModel.Password = HashedPassword;

        var newUser = _mapper.Map<User>(userRegisterModel);
        await _userRepository.Insert(newUser);

        LoginCredentials loginCredentials = new LoginCredentials
        {
            Email = userRegisterModel.Email,
            Password = userRegisterModel.Password
        };

        return await Login(loginCredentials);
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