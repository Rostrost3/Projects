using Microsoft.JSInterop;

namespace UserInterface.Storage
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IJSRuntime _runTime;

        private readonly string keyName = "JWT";

        public LocalStorage(IJSRuntime runTime)
        {
            _runTime = runTime;
        }

        public async Task<string?> GetKeyAsync()
        {
            return await _runTime.InvokeAsync<string?>("localStorage.getItem", keyName);
        }

        public async Task SetKeyAsync(string key)
        {
            await _runTime.InvokeVoidAsync("localStorage.setItem", keyName, key);
        }

        public async Task RemoveKeyAsync()
        {
            await _runTime.InvokeVoidAsync("localStorage.removeItem", keyName);
        }

        public async Task<bool> HasKeyAsync()
        {
            return !string.IsNullOrEmpty(await GetKeyAsync());
        }
    }
}
