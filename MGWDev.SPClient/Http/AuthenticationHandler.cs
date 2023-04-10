using MGWDev.SPClient.Authentication;
using MGWDev.SPClient.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Http
{
    public class AuthenticationHandler : HttpClientHandler
    {
        private IRequestAuthenticator _authenticator;

        public AuthenticationHandler(IRequestAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, CancellationToken cancellationToken)
        {
            await _authenticator.AuthenticateRequest(message);
            return await base.SendAsync(message, cancellationToken);
        }
    }
}
