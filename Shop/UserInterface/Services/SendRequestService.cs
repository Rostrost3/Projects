using Azure;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text;
using UserInterface.Storage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserInterface.Services
{
    public class SendRequestService
    {
        private readonly IHttpClientFactory _client;

        private readonly ILocalStorage _localStorage;

        private readonly NavigationManager _navManager;

        public SendRequestService(IHttpClientFactory client, ILocalStorage localStorage, NavigationManager navManager)
        {
            _client = client;
            _localStorage = localStorage;
            _navManager = navManager;
        }

        public async Task<HttpResponseMessage?> SendAsync(HttpMethod method, string requestUri, string? jsonBody = null)
        {
            try
            {
                using var request = new HttpRequestMessage(method, requestUri);

                var key = await _localStorage.GetKeyAsync();
                request.Headers.Add("Authorization", "bearer " + (key is not null ? key : ""));

                if(jsonBody != null)
                {
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                }

                var client = _client.CreateClient("api");

                var response = await client.SendAsync(request);

                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _navManager.NavigateTo("/login");
                    await _localStorage.RemoveKeyAsync();
                }

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[SendRequestService] ERROR {response.StatusCode}: {error}");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SendRequestService] ERROR {ex.Message}");
                _navManager.NavigateTo("/login");
                await _localStorage.RemoveKeyAsync();
                return null;
            }
        }
    }
}
