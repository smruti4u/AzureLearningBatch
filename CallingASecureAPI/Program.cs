using Microsoft.Azure.KeyVault;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CallingASecureAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var token = await GetTokenMSAL().ConfigureAwait(false);
            KeyVaultClient client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetTokenMSAL));
            var secret = await client.GetSecretAsync("https://vaultle.vault.azure.net/secrets/CosmosConnectionString/").ConfigureAwait(false);
            Console.WriteLine(secret.Value);
           // GraphServiceClient graphService = new GraphServiceClient(new DelegateAuthenticationProvider(async (request) => {

            //     request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            // }));

            //var result =  await graphService.Users.Request().GetAsync();

            Console.Read();
        }

        //private async static Task<string> GetToken()
        //{
        //    //const string CLIENTID = "8942c8c8-7d21-4a40-93cb-1175f61cb9a7";
        //    //const string CLIENTSECRET = "0BD6QJSbRGOAG5.Y_7S~8MZiTLbJQb6.1W";

        //    //var authority = "https://login.microsoftonline.com/b31ab4d1-cad2-4732-9872-ca352535e647";

        //    //AuthenticationContext authContext = new AuthenticationContext(authority);
        //    //ClientCredential credential = new ClientCredential(CLIENTID, CLIENTSECRET);
        //    //var result = await authContext.AcquireTokenAsync("https://graph.microsoft.com/", credential).ConfigureAwait(false);

        //    //result = result ?? throw new InvalidOperationException("Error Obtaining JWt token");

        //    //return result.AccessToken;

        //}

        private async static Task<string> GetTokenMSAL(string a, string b, string c)
        {
            const string CLIENTID = "cf69153e-77e5-4a67-8ba0-87416c16d20e";
            const string CLIENTSECRET = "~7NpaN3e6-Txq_2d.ab6EA6-os_Ci-RkJP";         

           var app =  ConfidentialClientApplicationBuilder.Create(CLIENTID)
                .WithAuthority(AzureCloudInstance.AzurePublic, "549a973a-fda2-4190-9ee8-67445857b006")
                .WithClientSecret(CLIENTSECRET).Build();

            var scopes = new List<string>() { "https://vault.azure.net/.default" };

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync().ConfigureAwait(false);


            return result.AccessToken;

        }
    }
}
