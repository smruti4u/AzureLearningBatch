using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace COSMOSDB.SDK
{
    class Program
    {
        static async  Task Main(string[] args)
        {
            var endPoint = "https://localhost:8081";
            var key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

            var client = new DocumentClient(new Uri(endPoint), key);

            var databaseDefination = new Database { Id = "vsdb" };
            await client.CreateDatabaseAsync(databaseDefination);

        }
    }
}
