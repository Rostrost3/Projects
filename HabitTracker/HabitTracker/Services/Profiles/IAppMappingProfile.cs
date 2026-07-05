using HabitTracker.DataBase.Entities;
using HabitTracker.Models;

namespace HabitTracker.Services.Profiles
{
    public interface IAppMappingProfile
    {
        User RegModelToUser(RegModel model);

        HabitModel HabitToHabitModel(Habit model);

        Habit HabitModelToHabit(HabitModel model);
    }
}
