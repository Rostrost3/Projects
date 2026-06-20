using Microsoft.JSInterop;

namespace UserInterface.Storage
{
    public interface ILocalStorage
    {
        public Task<string?> GetKeyAsync();

        public Task SetKeyAsync(string key);

        public Task RemoveKeyAsync();

        public Task<bool> HasKeyAsync();
    }
}
