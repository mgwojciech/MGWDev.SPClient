using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.SPClient.Utilities
{
    public static class StringUtilities
    {
        public static string GetDefaultScopeFromUrl(string url)
        {
            return $"{GetDomainFromSiteUrl(url)}/.default";
        }
        public static string GetDomainFromSiteUrl(string siteUrl)
        {
            Uri resourceUri = new Uri(siteUrl);
            return $"{resourceUri.Scheme}://{resourceUri.Host}";
        }
        public static string GetTenantName(string siteUrl)
        {
            if (string.IsNullOrEmpty(siteUrl))
                return string.Empty;

            var tokens = siteUrl.Split(new[] { "https://", ".sharepoint" }, StringSplitOptions.RemoveEmptyEntries);
            return tokens.Length <= 1 ? null : tokens[0];
        }

        public static string GetTenantAdminUrl(string siteUrl)
        {
            string tenantName = GetTenantName(siteUrl);

            if (string.IsNullOrEmpty(tenantName))
                return siteUrl;

            var result = $"https://{tenantName}-admin.sharepoint.com";
            return result;
        }
        public static string CombinePaths(this string value, params string[] secondValues)
        {
            string result = value;
            foreach (string val in secondValues)
            {
                result = BuildUrlFromSiteRelative(result, val);
            }
            return result;
        }
        public static string BuildUrlFromSiteRelative(string siteUrl, string siteRelativeUrl)
        {
            if (string.IsNullOrEmpty(siteRelativeUrl))
                return siteUrl;

            if (siteUrl.EndsWith("/"))
                siteUrl = siteUrl.Trim('/');
            if (siteRelativeUrl.StartsWith("/"))
                siteRelativeUrl = siteRelativeUrl.Substring(1);

            return $"{siteUrl}/{siteRelativeUrl}";
        }
    }
}
