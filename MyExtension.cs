using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace MyIP
{
    public static class MyExtension
    {
        public static string[] GetIpAddressesFromWebContent(this string str)
        {
            var ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            var result = ip.Matches(str);

            return (from object singleResult in result select singleResult.ToString()).ToArray();
        }
        public static string GetWebContent(this string url)
        {
            var client = new WebClient();
            return client.DownloadString(url);
        }
    }
}
