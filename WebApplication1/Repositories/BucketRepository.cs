using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System.Security.AccessControl;

namespace WebApplication1.Repositories
{
    public class BucketRepository
    {
        private string _projectId;
        private string _bucketName;
        public BucketRepository(string projectId, string bucketName)
        {
            _projectId = projectId;
            _bucketName = bucketName;
        }
        public async Task<string> UploadFileAsync(IFormFile file, string destinationPath)
        {
            var storage = StorageClient.Create();
            using (var stream = file.OpenReadStream())
            {
                var obj = await storage.UploadObjectAsync(_bucketName, destinationPath, null, stream);
                return $"https://storage.googleapis.com/{_bucketName}/{destinationPath}";
            }
        }

        public string AssignPermission(string userEmail, string objectName, string role = "READER")
        {
            var storage = StorageClient.Create();
            var storageObject = storage.GetObject(_bucketName, objectName, new GetObjectOptions
            {
                Projection = Projection.Full
            });

            storageObject.Acl.Add(new ObjectAccessControl
            {
                Bucket = _bucketName,
                Entity = $"user-{userEmail}",
                Role = role,
            });
            var updatedObject = storage.UpdateObject(storageObject);
            return updatedObject.SelfLink;
            //https://storage.cloud.google.com/swd63bpfc2026glrav1/finegrainedtest.png
        }

    }
}
