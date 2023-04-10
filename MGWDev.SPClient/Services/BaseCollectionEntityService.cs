using MGWDev.SPClient.Model;
using MGWDev.SPClient.Utilities.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Services
{
    public class BaseCollectionEntityService<T>
    {

        protected SelectQueryMapper SelectQueryMapper { get; set; } = new SelectQueryMapper();
        protected FilterQueryMapper FilterQueryMapper { get; set; } = new FilterQueryMapper();
        protected HttpClient SPClient { get; set; }
        public string ApiPath { get; set; }
        public int Top { get; set; } = 25;
        public bool IsNextPageAvailable
        {
            get
            {
                return !String.IsNullOrEmpty(NextPageLink);
            }
        }
        protected string? NextPageLink { get; set; }
        public bool IsPreviousPageAvailable
        {
            get
            {
                return !String.IsNullOrEmpty(PreviousPageLink);
            }
        }
        protected string? PreviousPageLink { get; set; }
        private string? _currentPageLink;
        public BaseCollectionEntityService(HttpClient spClient, string apiPath)
        {
            SPClient = spClient;
            ApiPath = apiPath;
        }
        public async Task<List<T>> Get(Expression<Func<T, bool>>? predicate = null)
        {
            NextPageLink = String.Empty;
            PreviousPageLink = String.Empty;
            _currentPageLink = BuildQuery(predicate);
            return await GetCurrentPage();
        }

        protected virtual async Task<List<T>> GetCurrentPage()
        {
            using (var response = await SPClient.GetAsync(_currentPageLink))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
                CollectionResponse<T>? result = await response.Content.ReadFromJsonAsync<CollectionResponse<T>>();
                if (result is null)
                {
                    throw new Exception("Unable to parse returned object");
                }
                NextPageLink = result.NextLink;
                return result.Value;
            }
        }

        public async Task<List<T>> GetNextPage()
        {
            PreviousPageLink = _currentPageLink;
            _currentPageLink = NextPageLink;
            return await GetCurrentPage();
        }
        public async Task<List<T>> GetPreviousPage()
        {
            _currentPageLink = PreviousPageLink;
            return await GetCurrentPage();
        }
        protected virtual string BuildQuery(Expression<Func<T, bool>>? predicate)
        {
            StringBuilder builder = new StringBuilder(ApiPath);
            var query = SelectQueryMapper.MapToSelectQuery<T>();
            builder.Append($"?$select={query.SelectQuery}");
            if (!String.IsNullOrEmpty(query.ExpandQuery))
            {
                builder.Append($"&$expand={query.ExpandQuery}");
            }
            if (predicate is not null)
            {
                builder.Append($"&$filter={FilterQueryMapper.BuildFilterQuery<T>(predicate)}");
            }
            if (Top > 0)
            {
                builder.Append($"&$top={Top}");
            }

            return builder.ToString();
        }
    }
}
