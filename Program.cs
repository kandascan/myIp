using System;
using System.Linq;
using DataAccess;

namespace MyIP
{
    class Program
    {
        private const string WebSite = "http://twojeip.wp.pl/";

        static void Main(string[] args)
        {
            try
            {
                var dateTimeNow = String.Format("{0:g}", DateTime.Now);
                var webContent = WebSite.GetWebContent();
                var ipAddresses = webContent.GetIpAddressesFromWebContent();
                var myIpAddress = RemoveDuplicateIpAddress(ipAddresses);
                var mongoDb = new MongoConnection();
                try
                {
                    var ipAddressesFromDatabase = mongoDb.GetListIpAddress();

                    if (ipAddressesFromDatabase.Count == 0)
                    {
                        EmailSender.SendEmail("Your IP address", myIpAddress);
                    }
                    else
                    {
                        if (myIpAddress.TrimEnd() != ipAddressesFromDatabase.Last().IpAddress)
                        {
                            EmailSender.SendEmail("Your IP address", myIpAddress);
                        }
                    }
                    
                    mongoDb.InsertIpAddressInMongoDatabase(new IpDataItem
                    {
                        _id = ipAddressesFromDatabase.Count == 0 ? "1" : (ipAddressesFromDatabase.Count+1).ToString(),
                        Date = dateTimeNow,
                        IpAddress = myIpAddress.TrimEnd(),
                    });
                }
                catch (Exception mongoException)
                {
                    var messageBody = String.Format("Your Ip address {0}\n Mongo error exceptio: {1}", myIpAddress,
                        mongoException);
                    EmailSender.SendEmail("Mongo Error", messageBody);
                }
                Console.WriteLine("[log Date] {0}\nYour Ip address: {1}", dateTimeNow, myIpAddress);
            }
            catch (Exception ex)
            {
                EmailSender.SendEmail("ERROR", ex.Message);
            }
         }

        static string RemoveDuplicateIpAddress(string[] myIp)
        {
            var distinctIp = myIp.Distinct();
            return distinctIp.Aggregate("", (current, ip) => current + (ip + "\n"));
        }
    }
}