using MGWDev.SPClient.Utilities;
using PnP.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Authentication
{
    public class PnPAuthenticator : IRequestAuthenticator
    {
        protected IAuthenticationProvider _authProvider;
        public Uri Resource { get; set; }
        public PnPAuthenticator(IAuthenticationProvider authProvider)
        {
            _authProvider = authProvider;
        }
        public async Task AuthenticateRequest(HttpRequestMessage request)
        {
            Uri resource = new Uri(StringUtilities.GetDomainFromSiteUrl(request.RequestUri.ToString()));
            if(Resource is not null)
            {
                resource = Resource;
            }
            try
            {
                await _authProvider.AuthenticateRequestAsync(resource, request);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
