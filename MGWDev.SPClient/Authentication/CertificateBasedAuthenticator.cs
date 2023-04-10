using MGWDev.SPClient.Utilities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Authentication
{
    public class CertificateBasedAuthenticator : IRequestAuthenticator
    {
        private IConfidentialClientApplication _app;
        public CertificateBasedAuthenticator(string clientId, 
            string certThumbprint, 
            string tenantId,
            AzureCloudInstance cloudInstance = AzureCloudInstance.AzurePublic,
            AadAuthorityAudience audience = AadAuthorityAudience.AzureAdMyOrg)
        {
            var appBuilder = ConfidentialClientApplicationBuilder.Create(clientId);
            var certificate = LoadCertificate(StoreName.My, StoreLocation.CurrentUser, certThumbprint);
            
            _app = appBuilder.
                WithCertificate(certificate).
                WithTenantId(tenantId).
                WithAuthority(cloudInstance,audience)
                .Build();

        }
        private X509Certificate2 LoadCertificate(StoreName storeName, StoreLocation storeLocation, string thumbprint)
        {
            // The following code gets the cert from the keystore
            using (X509Store store = new X509Store(storeName, storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                X509Certificate2Enumerator enumerator = certCollection.GetEnumerator();
                X509Certificate2 cert = null;

                while (enumerator.MoveNext())
                {
                    cert = enumerator.Current;
                }

                return cert;
            }
        }

        public async Task AuthenticateRequest(HttpRequestMessage request)
        {
            string scope = StringUtilities.GetDefaultScopeFromUrl(request.RequestUri.ToString());
            var authResult = await _app.AcquireTokenForClient(new string[] { scope }).ExecuteAsync();
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + authResult.AccessToken);
        }
    }
}
