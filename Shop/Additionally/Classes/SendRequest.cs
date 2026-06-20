using System;
using System.Collections.Generic;
using System.Text;

namespace Additionally.Classes
{
    public class SendRequest
    {
        public async Task SendAsync(HttpMethod method, string requestUri)
        {
            using var request = new HttpRequestMessage(method, requestUri);

            var client = 
        }
    }
}
