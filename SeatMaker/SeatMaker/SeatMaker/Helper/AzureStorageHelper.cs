using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeatMaker.Helper
{
    public static class AzureStorageHelper
    {
        public static async Task<string> UpdateFileAsync(string file)
        {
            var c = new StorageCredentials("kscloudapidev2", "LguezTtdcX9+dwXfv+PboCV/PSWqUrkKVO4axuZzcMe5e9xHCVq/zjwx2WOYUNwv5t+Z/Py8ncdO7UNgGdU3JQ==");
            var client = new CloudStorageAccount(c, true);
            var uri = client.BlobStorageUri;
            var blobClient = client.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("procon2019-mukaida");
            if (!(await container.ExistsAsync()))
                return null;
            var blob = container.GetBlockBlobReference(DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
            await blob.UploadFromFileAsync(file);
            return blob.Uri.AbsoluteUri;
        }
    }
}
