using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Authentication
{
    public class AppOnlyAuthenticationProvider : IRequestAuthenticator
    {
        public Task AuthenticateRequest(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
