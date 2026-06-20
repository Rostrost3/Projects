using DTO.DTOEntities;
using DTO.Entities;
using System.Text.Json;

namespace UserInterface.Services
{
    public class RoleService
    {
        private readonly SendRequestService _sendRequestService;

        private readonly ForceExitService _forceExitService;

        public RoleService(SendRequestService sendRequestService, ForceExitService forceExitService)
        {
            _sendRequestService = sendRequestService;
            _forceExitService = forceExitService;
        }

        public async Task<bool> IsAdmin()
        {
            using var response = await _sendRequestService.SendAsync(HttpMethod.Get, "api/user/getuser");

            if(response!.IsSuccessStatusCode)
            {
                try
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var userDTO = await JsonSerializer.DeserializeAsync<UserDTO>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return userDTO!.RoleId == UserRoles.Admin;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR {ex.Message}");
                    await _forceExitService.Exit();
                }
            }
            return false;
        }
    }
}
