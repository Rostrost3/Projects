using Microsoft.AspNetCore.Components;
using UserInterface.Storage;

namespace UserInterface.Services
{
    public class ForceExitService
    {
        private readonly NavigationManager _navManager;

        private readonly ILocalStorage _localStorage;

        public ForceExitService(NavigationManager navManager, ILocalStorage localStorage)
        {
            _navManager = navManager;
            _localStorage = localStorage;
        }

        public async Task Exit()
        {
            _navManager.NavigateTo("/login");
            await _localStorage.RemoveKeyAsync();
        }
    }
}
