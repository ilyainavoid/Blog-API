using AutoMapper;
using BlogApi.Migrations;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Repositories;
using BlogApi.Repositories.Interfaces;

namespace BlogApi.Services
{
    public class UserService : IUserService
    {
        private readonly TokenUtilities _tokenUtility;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<ExpiredToken> _expTokenRepository;
        private readonly IMapper _mapper;

        public UserService(TokenUtilities tokenUtility, IUserRepository userRepository, IMapper mapper, IBaseRepository<ExpiredToken> expTokenRepository)
        {
            _tokenUtility = tokenUtility;
            _userRepository = userRepository;
            _mapper = mapper;
            _expTokenRepository = expTokenRepository;
        }

        public async Task<TokenResponse> Register(UserRegisterModel userRegisterModel)
        {
            var user = await _userRepository.GetUserByEmail(userRegisterModel.Email);
            if (user != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterModel.Password, salt);
            var insecurePassword = userRegisterModel.Password;
            userRegisterModel.Password = hashedPassword;

            var newUser = _mapper.Map<User>(userRegisterModel);
            await _userRepository.Insert(newUser);

            LoginCredentials loginCredentials = new LoginCredentials
            {
                Email = userRegisterModel.Email,
                Password = insecurePassword
            };

            return await Login(loginCredentials);
        }

        public async Task<TokenResponse> Login(LoginCredentials credentials)
        {
            var user = await _userRepository.GetUserByEmail(credentials.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);

            if (!isValidPassword)
            {
                throw new Exception("Invalid password.");
            }

            var token = _tokenUtility.GenerateToken(user);
            return new TokenResponse(token);
        }

        public async Task Logout(string token)
        {
            var expiredToken = new ExpiredToken
            {
                Token = token
            };
            
            await _expTokenRepository.Insert(expiredToken);
        }

        public async Task<UserDto> GetProfileInfo(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task EditProfileInfo(Guid id, UserEditModel userEditModel)
        {
            throw new NotImplementedException();
        }
    }
}
