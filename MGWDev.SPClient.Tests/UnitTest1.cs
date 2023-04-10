using AngleSharp.Io;
using MGWDev.SPClient.Authentication;
using MGWDev.SPClient.Http;
using MGWDev.SPClient.Model;
using MGWDev.SPClient.Services;
using MGWDev.SPClient.Tests.Model;
using PnP.Core.Auth;
using System.Net.Http.Json;

namespace MGWDev.SPClient.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            CertificateBasedAuthenticator authenticator = new CertificateBasedAuthenticator("774d80db-4b4d-4bc5-878f-a6a191a0a7f6", "415BBD4207C0B87ED70C9FA1D123D32DB0EF44AC", "6bf6c4ac-66c5-453d-b839-578a5c34990c");

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com/sites/tea-point", authenticator);
            using (var response = await client.GetAsync("/_api/site"))
            {
                SPSite? site = await response.Content.ReadFromJsonAsync<SPSite>();
                Assert.IsNotNull(site);
            }
        }
        [TestMethod]
        public async Task GetSiteWithSiteService()
        {
            CertificateBasedAuthenticator authenticator = new CertificateBasedAuthenticator("774d80db-4b4d-4bc5-878f-a6a191a0a7f6", "415BBD4207C0B87ED70C9FA1D123D32DB0EF44AC", "6bf6c4ac-66c5-453d-b839-578a5c34990c");

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com/sites/tea-point", authenticator);
            SiteService service = new SiteService(client);

            SPSite site = await service.Get();
            Assert.IsNotNull(site);
        }
        [TestMethod]
        public async Task GetWebWithSiteService()
        {
            CertificateBasedAuthenticator authenticator = new CertificateBasedAuthenticator("774d80db-4b4d-4bc5-878f-a6a191a0a7f6", "415BBD4207C0B87ED70C9FA1D123D32DB0EF44AC", "6bf6c4ac-66c5-453d-b839-578a5c34990c");

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com/sites/tea-point", authenticator);
            BaseSPEntityService<SPWeb> service = new BaseSPEntityService<SPWeb>(client, "/_api/web");

            SPWeb site = await service.Get();
            Assert.IsNotNull(site);
        }
        [TestMethod]
        public async Task GetListsWithQuery()
        {
            CertificateBasedAuthenticator authenticator = new CertificateBasedAuthenticator("774d80db-4b4d-4bc5-878f-a6a191a0a7f6", "415BBD4207C0B87ED70C9FA1D123D32DB0EF44AC", "6bf6c4ac-66c5-453d-b839-578a5c34990c");

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com", authenticator);

            BaseCollectionEntityService<SPList> service = new BaseCollectionEntityService<SPList>(client, "/sites/tea-point/_api/web/lists");
            List<SPList> lists = await service.Get(list => list.IsHidden == true);
            Assert.IsNotNull(lists);
        }
        [TestMethod]
        public async Task GetInformationMessages()
        {
            CertificateBasedAuthenticator pnpAuthenticator = new CertificateBasedAuthenticator("774d80db-4b4d-4bc5-878f-a6a191a0a7f6", "415BBD4207C0B87ED70C9FA1D123D32DB0EF44AC", "6bf6c4ac-66c5-453d-b839-578a5c34990c");

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com", pnpAuthenticator);

            BaseCollectionEntityService<InformationMessage> service = new BaseCollectionEntityService<InformationMessage>(client, "/sites/tea-point/_api/web/lists/getByTitle('InformationMessages')/items");
            List<InformationMessage> messages = await service.Get(message => message.EndDate <= DateTime.UtcNow || message.Title == "Test 6-11-2020-1");

            Assert.IsNotNull(messages);
        }
        [TestMethod]
        public async Task GetInformationMessages2()
        {
            CertificateBasedAuthenticator authenticator = new CertificateBasedAuthenticator("774d80db-4b4d-4bc5-878f-a6a191a0a7f6", "415BBD4207C0B87ED70C9FA1D123D32DB0EF44AC", "6bf6c4ac-66c5-453d-b839-578a5c34990c");

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com", authenticator);

            BaseCollectionEntityService<InformationMessage> service = new BaseCollectionEntityService<InformationMessage>(client, "/sites/tea-point/_api/web/lists/getByTitle('InformationMessages')/items");
            List<InformationMessage> messages = await service.Get(message => message.Title == "Test 6-11-2020-1");

            Assert.IsNotNull(messages);
        }
        [TestMethod]
        public async Task GetInformationMessages3()
        {
            CertificateBasedAuthenticator authenticator = new CertificateBasedAuthenticator("774d80db-4b4d-4bc5-878f-a6a191a0a7f6", "415BBD4207C0B87ED70C9FA1D123D32DB0EF44AC", "6bf6c4ac-66c5-453d-b839-578a5c34990c");

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com", authenticator);

            BaseCollectionEntityService<InformationMessage> service = new BaseCollectionEntityService<InformationMessage>(client, "/sites/tea-point/_api/web/lists/getByTitle('InformationMessages')/items");
            List<InformationMessage> messages = await service.Get(message => message.Author.Id == 7);

            Assert.IsNotNull(messages);
        }
        //[TestMethod]
        public async Task GetSiteWithPnPAuth()
        {
            InteractiveAuthenticationProvider provider = new InteractiveAuthenticationProvider("99bf0f5c-99de-43fd-9815-036a1ebcb01c", "6bf6c4ac-66c5-453d-b839-578a5c34990c", new Uri("http://localhost"));
            PnPAuthenticator authenticator = new PnPAuthenticator(provider);

            HttpClient client = HttpClientFactory.GetHttpClient("https://mwdevvalo.sharepoint.com/sites/tea-point", authenticator);
            using (var response = await client.GetAsync("/_api/site"))
            {
                SPSite? site = await response.Content.ReadFromJsonAsync<SPSite>();
                Assert.IsNotNull(site);
            }
        }
    }
}