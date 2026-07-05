using HabitTracker.DataBase.Entities;
using HabitTracker.DataBase.Repositories;
using HabitTracker.Models;
using HabitTracker.Services.JWT;
using HabitTracker.Services.PasswordHashing;
using HabitTracker.Services.Profiles;

namespace HabitTracker.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;

        private readonly IAppMappingProfile _appMappingProfile;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IJWTProvider _jwtProvider;

        public AuthService(IRepository<User> userRepository, IAppMappingProfile appMappingProfile, IPasswordHasher passwordHasher, IJWTProvider jwtProvider)
        {
            _userRepository = userRepository;
            _appMappingProfile = appMappingProfile;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<ServiceResult<string>> RegisterAsync(RegModel regModel)
        {
            User? user = await _userRepository.GetOneByFuncAsync(x => x.Email == regModel.Email);
            if (user != null)
            {
                return new ServiceResult<string> { ErrorMessage = "User with this email already exist" };
            }
            user = _appMappingProfile.RegModelToUser(regModel);
            (string hash, string salt) = _passwordHasher.HashPassword(user.PasswordHash);
            user.PasswordHash = hash;
            user.Salt = salt;
            await _userRepository.CreateAsync(user);
            return new ServiceResult<string> { Data = _jwtProvider.GenerateToken(user.Id, user.Name) };
        }

        public async Task<ServiceResult<string>> LoginAsync(LoginModel loginModel)
        {
            User? user = await _userRepository.GetOneByFuncAsync(x => x.Email == loginModel.Email);
            if (user == null)
            {
                return new ServiceResult<string> { ErrorMessage = "User with this email doesn't exist" };
            }
            if (!_passwordHasher.VerifyPassword(loginModel.Password, user.PasswordHash, user.Salt))
            {
                return new ServiceResult<string> { ErrorMessage = "WrongPassword" };
            }
            return new ServiceResult<string> { Data = _jwtProvider.GenerateToken(user.Id, user.Name) };
        }
    }
}
