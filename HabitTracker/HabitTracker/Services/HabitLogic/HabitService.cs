using HabitTracker.DataBase;
using HabitTracker.DataBase.Entities;
using HabitTracker.DataBase.Repositories;
using HabitTracker.Models;
using HabitTracker.Services.Profiles;

namespace HabitTracker.Services.HabitLogics
{
    public class HabitService : IHabitService
    {
        private readonly IRepository<Habit> _habitRepository;

        private readonly IAppMappingProfile _appMappingProfile;

        public HabitService(IRepository<Habit> habitRepository, IAppMappingProfile appMappingProfile)
        {
            _habitRepository = habitRepository;
            _appMappingProfile = appMappingProfile;
        }

        private HabitModel UpdateHabitCheck(HabitModel habitModel)
        {
            if(DateTime.UtcNow.Subtract(habitModel.ResetTime).TotalSeconds > 0)
            {
                habitModel.IsChecked = false;
            }
            return habitModel;
        }

        public async Task<ServiceResult<List<HabitModel>>> GetAllByUserIdAsync(int userId)
        {
            List<Habit> habits = await _habitRepository.GetAllByFuncAsync(x => x.UserId == userId);
            List<HabitModel> res = habits.Select(x => _appMappingProfile.HabitToHabitModel(x)).Select(x => UpdateHabitCheck(x)).ToList();
            return new ServiceResult<List<HabitModel>> { Data = res };
        }

        public async Task<ServiceResult<HabitModel>> GetByIdAsync(int id, int userId)
        {
            Habit? habit = await _habitRepository.GetOneByFuncAsync(x => x.Id == id && x.UserId == userId);
            HabitModel res = habit == null ? new() : _appMappingProfile.HabitToHabitModel(habit);
            return new ServiceResult<HabitModel> { Data = res };
        }

        public async Task<ServiceResult<string>> CreateAsync(HabitModel habitModel, int userId)
        {
            Habit habit = _appMappingProfile.HabitModelToHabit(habitModel);
            habit.UserId = userId;
            habit.ResetTime = DateTime.UtcNow.Date.AddDays(1);
            await _habitRepository.CreateAsync(habit);
            return new ServiceResult<string> { Data = "" };
        }

        public async Task<ServiceResult<string>> UpdateAsync(HabitModel habitModel, int userId)
        {
            Habit? habit = await _habitRepository.GetOneByFuncAsync(x => x.Id == habitModel.Id && x.UserId == userId);
            if(habit != null)
            {
                habit.Name = habitModel.Name;
                if(habit.IsChecked != habitModel.IsChecked)
                {
                    habit.IsChecked = habitModel.IsChecked;
                    habit.ResetTime = DateTime.UtcNow.Date.AddDays(1);
                }
                await _habitRepository.UpdateAsync(habit);
                return new ServiceResult<string> { Data = "" };
            }
            return new ServiceResult<string> { ErrorMessage = "There is no such habit" };
        }

        public async Task<ServiceResult<string>> DeleteAsync(int id, int userId)
        {
            Habit? habit = await _habitRepository.GetOneByFuncAsync(x => x.Id == id && x.UserId == userId);
            if(habit != null)
            {
                await _habitRepository.DeleteAsync(habit);
                return new ServiceResult<string> { Data = "" };
            }
            return new ServiceResult<string> { ErrorMessage = "There is no such habit" };
        }
    }
}
