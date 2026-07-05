namespace HabitTracker.DataBase.Entities
{
    public class Habit
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsChecked { get; set; } = false;

        public DateTime ResetTime { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
