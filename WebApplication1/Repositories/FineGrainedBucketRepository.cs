
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace WebApplication1.Repositories
{
    public class FineGrainedBucketRepository : IBucketRepository
    {

        private readonly string _projectId;
        private readonly string _bucketName;
        public FineGrainedBucketRepository()
        {
            _projectId = "swd63bpfc2026";
            _bucketName = "swd63bpfc2026glrav1";
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

        public async Task<string> AssignPermission(string userEmail, string objectName, string role = "READER")
        {
            var storage = StorageClient.Create();
            var storageObject = await storage.GetObjectAsync(_bucketName, objectName, new GetObjectOptions
            {
                Projection = Projection.Full
            });

            storageObject.Acl.Add(new ObjectAccessControl
            {
                Bucket = _bucketName,
                Entity = $"user-{userEmail}",
                Role = role,
            });
            var updatedObject = await storage.UpdateObjectAsync(storageObject);
            return $"https://storage.cloud.google.com/{_bucketName}/{objectName}";
            //https://storage.cloud.google.com/swd63bpfc2026glrav1/5ebee7d9-ddb5-4f92-92d2-dfa727ce00e0.csv
        }
    }
}
