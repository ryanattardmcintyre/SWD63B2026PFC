using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System.Security.AccessControl;

namespace WebApplication1.Repositories
{
    //1. change name of BucketRepository to UniformBucketRepository
    //2. Created an interface called IBucketRepository with method UploadFileAsync
    //3. Inherit UniformBucketRepository from IBucketRepository
    //4. Create a class called FineGrainedBucketRepository that also inherits from IBucketRepository
    //    and implements the methods UploadFileAsync and AssignPermission
    //5. Amend accordingly the program.cs using KeyedScoped registration type
         //Note: KeyedScoped allows us to use both implementations together in a single controller/method

    public class UniformBucketRepository: IBucketRepository
    {
        private string _projectId;
        private string _bucketName;
        public UniformBucketRepository()
        {
            _projectId = "swd63bpfc2026";
            _bucketName = "swd63bpfc2026rav2";
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

      

    }
}
