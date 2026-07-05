namespace HabitTracker.Models
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; } 

        public string? ErrorMessage { get; set; }
    }
}
