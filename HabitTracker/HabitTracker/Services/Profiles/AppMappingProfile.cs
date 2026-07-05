using HabitTracker.DataBase.Entities;
using HabitTracker.Models;

namespace HabitTracker.Services.Profiles
{
    public class AppMappingProfile : IAppMappingProfile
    {
        public User RegModelToUser(RegModel model)
        {
            User user = new User()
            {
                Email = model.Email,
                Name = model.Name,
                PasswordHash = model.Password
            };
            return user;
        }

        public HabitModel HabitToHabitModel(Habit model)
        {
            HabitModel habitModel = new HabitModel()
            {
                Id = model.Id,
                Name = model.Name,
                IsChecked = model.IsChecked,
                ResetTime = model.ResetTime
            };
            return habitModel;
        }

        public Habit HabitModelToHabit(HabitModel model)
        {
            Habit habit = new Habit()
            {
                Id = model.Id,
                Name = model.Name,
                IsChecked = model.IsChecked,
                ResetTime = model.ResetTime
            };
            return habit;
        }
    }
}
