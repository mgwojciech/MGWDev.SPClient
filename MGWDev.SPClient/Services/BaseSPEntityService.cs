using MGWDev.SPClient.Utilities.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Services
{
    public class BaseSPEntityService<T>
    {
        protected HttpClient SPClient { get; set; }
        public string ApiPath { get; set; }
        protected SelectQueryMapper SelectQueryMapper { get; set; } = new SelectQueryMapper();
        public BaseSPEntityService(HttpClient spClient, string apiPath)
        {
            SPClient = spClient;
            ApiPath = apiPath;
        }

        public virtual async Task<T> Get()
        {
            string selectQuery = $"?$select={SelectQueryMapper.MapToSelectQuery<T>().SelectQuery}";
            using(var response = await SPClient.GetAsync($"{ApiPath}{selectQuery}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
                T? result = await response.Content.ReadFromJsonAsync<T>();
                if(result is null)
                {
                    throw new Exception("Unable to parse returned object");
                }
                return result;
            }
        }
    }
}
