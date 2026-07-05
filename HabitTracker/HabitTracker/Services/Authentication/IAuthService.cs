using HabitTracker.Models;

namespace HabitTracker.Services.Authentication
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> RegisterAsync(RegModel regModel);

        Task<ServiceResult<string>> LoginAsync(LoginModel loginModel);
    }
}
