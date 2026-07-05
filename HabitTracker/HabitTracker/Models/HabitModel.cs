namespace HabitTracker.Models
{
    public class HabitModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsChecked { get; set; } = false;

        public DateTime ResetTime { get; set; }
    }
}
