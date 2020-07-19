using System;
using System.Net.Http;
using System.Text;

namespace CallingWebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            string toEnocde = "$firstwebappappservice:x2rGGLPtgpkLandCmP2ueWFas89pqZppEagAlwKN8n8sy2XPdBHvNYd1pbLD";
            byte[] data = ASCIIEncoding.ASCII.GetBytes(toEnocde);
            string encodedadata = Convert.ToBase64String(data);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedadata}");
            var response = client.PostAsync("https://firstwebappappservice.scm.azurewebsites.net/api/triggeredwebjobs/CleanUpLogFile/run", null).GetAwaiter().GetResult();
        }
    }
}
