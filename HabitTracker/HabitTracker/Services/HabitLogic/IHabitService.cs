using HabitTracker.Models;

namespace HabitTracker.Services.HabitLogics
{
    public interface IHabitService
    {
        Task<ServiceResult<List<HabitModel>>> GetAllByUserIdAsync(int userId);

        Task<ServiceResult<HabitModel>> GetByIdAsync(int id, int userId);

        Task<ServiceResult<string>> CreateAsync(HabitModel habitModel, int userId);

        Task<ServiceResult<string>> UpdateAsync(HabitModel habitModel, int userId);

        Task<ServiceResult<string>> DeleteAsync(int id, int userId);
    }
}
