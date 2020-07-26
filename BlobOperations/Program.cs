using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlobOperations
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string fileToUpload = "input.txt";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=azstorageaccountlearn;AccountKey=AWU6npF4KDZm2cL29Rxpztz3KEaR9Hnc9CqysUzBkNdN84JyStD9UfQzt76pKUXeBw/gGBlDlbjHk8jsVaht4w==;EndpointSuffix=core.windows.net");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("vscontainernew");
            try
            {
                container.CreateIfNotExists();
            }
            catch (Exception exe)
            {

            }
            
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileToUpload);
            
            blockBlob.UploadFromFile(fileToUpload);

            var token = GetSaasToken(blockBlob);
            blockBlob.Metadata["Author"] = "Client Code";
            blockBlob.Metadata["Priority"] = "High";
            blockBlob.SetMetadata();
            blockBlob.CreateSnapshot();
            blockBlob.DownloadToFile(string.Format("./CopyOf{0}", fileToUpload), System.IO.FileMode.Create);


            await HandleConCurrencyUpdate(blockBlob, ConCurrencyType.Pessimistic);

            //blockBlob.Delete();
        }

        private static string GetSaasToken(CloudBlockBlob blob)
        {
            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.Now.AddDays(1),
            };
            var sassToken = blob.GetSharedAccessSignature(policy);
            return sassToken;
        }

        public static CloudBlockBlob GetBlob()
        {
            const string fileToUpload = "input.txt";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=azstorageaccountlearn;AccountKey=AWU6npF4KDZm2cL29Rxpztz3KEaR9Hnc9CqysUzBkNdN84JyStD9UfQzt76pKUXeBw/gGBlDlbjHk8jsVaht4w==;EndpointSuffix=core.windows.net");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("vscontainernew");
            try
            {
                container.CreateIfNotExists();
            }
            catch (Exception exe)
            {

            }
            
            var blob = container.GetBlockBlobReference(fileToUpload);
            blob.FetchAttributes();
            return blob;
            
        }

        public static async Task<bool> HandleConCurrencyUpdate(CloudBlockBlob blob, ConCurrencyType type)
        {
            bool result = false;
            switch(type)
            {
                case ConCurrencyType.Default:
                    blob.Metadata["Author"] = "Client Code";
                    blob.Metadata["Priority"] = "High";
                    blob.SetMetadata();
                    break;
                case ConCurrencyType.Optimistic:
                    blob.Metadata["Author"] = "Client Code";
                    blob.Metadata["Priority"] = "High";
                    string leaseId = await blob.AcquireLeaseAsync(TimeSpan.FromMinutes(1));
                    var accessCondition = new AccessCondition
                    {
                        LeaseId = leaseId
                    };
                    await blob.SetMetadataAsync(accessCondition, null, null);
                    break;
                case ConCurrencyType.Pessimistic:
                    blob.Metadata["Author"] = "Client Code";
                    blob.Metadata["Priority"] = "High";
                    var accessCondition1 = new AccessCondition
                    {
                        IfMatchETag = blob.Properties.ETag
                    };
                    try
                    {
                        await blob.SetMetadataAsync(accessCondition1, null, null);
                    }
                    catch(Exception exe)
                    {
                        var cloudBlob = GetBlob();
                        await HandleConCurrencyUpdate(cloudBlob, ConCurrencyType.Pessimistic);
                        result = false;
                    }
                    break;

            }

            return result;
        }
    }
}
