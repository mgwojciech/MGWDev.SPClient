using MGWDev.SPClient.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Http
{
    public class HttpClientFactory
    {
        public static HttpClient GetHttpClient(string siteUrl, IRequestAuthenticator authenticator)
        {
            HttpClient httpClient = new HttpClient(new AuthenticationHandler(authenticator));
            httpClient.BaseAddress = new Uri(siteUrl);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

            return httpClient;
        }
    }
}
