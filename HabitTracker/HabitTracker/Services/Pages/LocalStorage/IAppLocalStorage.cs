namespace HabitTracker.Services.Pages.LocalStorage
{
    public interface IAppLocalStorage
    {
        Task<string?> GetTokenAsync();

        Task SetTokenAsync(string value);

        Task DeleteTokenAsync();
    }
}
