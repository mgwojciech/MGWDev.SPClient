using MGWDev.SPClient.Utilities.OData;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MGWDev.SPClient.Graph.Users
{
    public static class UsersRequestBuilderExtensions
    {
        public static async Task<UserCollectionResponse?> GetWithFilterAsync(this UsersRequestBuilder builder, Expression<Func<User,bool>> expression)
        {
            FilterQueryMapper mapper = new FilterQueryMapper();
            string filterQuery = mapper.BuildFilterQuery(expression);
            return await builder.GetAsync((Microsoft.Graph.Users.UsersRequestBuilder.UsersRequestBuilderGetRequestConfiguration config) =>
            {
                config.QueryParameters.Filter = HttpUtility.UrlDecode(filterQuery);
            });
        }
    }
}
