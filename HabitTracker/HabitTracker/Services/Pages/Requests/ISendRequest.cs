namespace HabitTracker.Services.Pages.Requests
{
    public interface ISendRequest
    {
        Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string requestUri, HttpContent? content = null);
    }
}
