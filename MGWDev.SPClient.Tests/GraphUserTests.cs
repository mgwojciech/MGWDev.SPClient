using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGWDev.SPClient.Graph.Users;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Graph.Models;

namespace MGWDev.SPClient.Tests
{
    [TestClass]
    public class GraphUserTests
    {
        [TestMethod]
        public async Task Given_SimpleExpression_Should_GetUser()
        {
            string expectedPath = "https://graph.microsoft.com/v1.0/users?%24filter=%28DisplayName%20eq%20%27Test%20User%27%29";
            Mock<HttpClientHandler> handler = new Mock<HttpClientHandler>();
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.AbsoluteUri == expectedPath
            ),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new UserCollectionResponse()
                    {
                        Value = new List<User>()
                          {
                              new User()
                              {
                                  DisplayName = "Test User"
                              }
                          }
                    })
                });


            var graphClient = new GraphServiceClient(new HttpClient(handler.Object));

            var users = await graphClient.Users.GetWithFilterAsync(user => user.DisplayName == "Test User");

            Assert.AreEqual("Test User", users.Value.First().DisplayName);
        }
    }
}
