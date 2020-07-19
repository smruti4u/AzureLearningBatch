using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.ComponentModel;

namespace BlobOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileToUpload = "input.txt";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=azstorageaccountlearn;AccountKey=AWU6npF4KDZm2cL29Rxpztz3KEaR9Hnc9CqysUzBkNdN84JyStD9UfQzt76pKUXeBw/gGBlDlbjHk8jsVaht4w==;EndpointSuffix=core.windows.net");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("vscontainer");
            try
            {
                container.CreateIfNotExists();
            }
            catch(Exception exe)
            {

            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileToUpload);
            blockBlob.UploadFromFile(fileToUpload);

            blockBlob.Metadata["Author"] = "Client Code";
            blockBlob.Metadata["Priority"] = "High";

            blockBlob.SetMetadata();
            blockBlob.CreateSnapshot();
            blockBlob.DownloadToFile(string.Format("./CopyOf{0}", fileToUpload), System.IO.FileMode.Create);

            blockBlob.Delete();


        }
    }
}
