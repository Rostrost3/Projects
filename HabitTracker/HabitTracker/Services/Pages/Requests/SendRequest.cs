using HabitTracker.Services.Pages.LocalStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net;
using System.Net.Http.Headers;

namespace HabitTracker.Services.Pages.Requests
{
    public class SendRequest : ISendRequest
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IAppLocalStorage _localStorage;

        public SendRequest(IHttpClientFactory httpClientFactory, IAppLocalStorage localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string requestUri, HttpContent? content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);

            var token = await _localStorage.GetTokenAsync();
            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if(content != null)
            {
                request.Content = content;
            }

            HttpClient client = _httpClientFactory.CreateClient("api");

            HttpResponseMessage response = await client.SendAsync(request);

            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _localStorage.DeleteTokenAsync();
            }

            return response;
        }
    }
}
