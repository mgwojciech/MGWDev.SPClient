using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Authentication
{
    public interface IRequestAuthenticator
    {
        Task AuthenticateRequest(HttpRequestMessage request);
    }
}
