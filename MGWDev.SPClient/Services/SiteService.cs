using MGWDev.SPClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Services
{
    public class SiteService<T> : BaseSPEntityService<T>
    {
        public SiteService(HttpClient spHttpClient) : base(spHttpClient, "/_api/site")
        {

        }
    }

    public class SiteService : SiteService<SPSite>
    {
        public SiteService(HttpClient spHttpClient) : base(spHttpClient)
        {

        }
    }
}
