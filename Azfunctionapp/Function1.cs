using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Azfunctionapp
{
    public static class Function1
    {
        [FunctionName("firstfunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("TimerTrigger")]
        [return: Table("functionTable")]
        public static MyPoco TimerTrigger(
    [TimerTrigger("0 */2 * * * *")] TimerInfo info,
    [Blob("%BlobName%", FileAccess.Read, Connection = "AzureWebJobsStorage")]Stream blob,
    ILogger log)
        {
            StreamReader streamReader = new StreamReader(blob);
            JObject jObject = JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd());

            return new MyPoco() {  Order = jObject };
        }

        public class MyPoco
        {
            public MyPoco()
            {
                this.PartitionKey = new Guid().ToString();
                this.RowKey = "result";
            }

            public string PartitionKey { get; set; }
            public string RowKey { get; set; }

            public JObject Order { get; set; }
        }
    }
}
