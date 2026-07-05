using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace HabitTracker.Services.Pages.LocalStorage
{
    public class AppLocalStorage : IAppLocalStorage
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        private const string Key = "user_token";

        public AppLocalStorage(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task<string?> GetTokenAsync()
        {
            var value = await _protectedLocalStorage.GetAsync<string>(Key);
            return value.Success ? value.Value : null;
        }

        public async Task SetTokenAsync(string value)
        {
            await _protectedLocalStorage.SetAsync(Key, value);
        }

        public async Task DeleteTokenAsync()
        {
            await _protectedLocalStorage.DeleteAsync(Key);
        }
    }
}
