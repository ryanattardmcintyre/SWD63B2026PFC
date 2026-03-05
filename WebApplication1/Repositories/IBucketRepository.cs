namespace WebApplication1.Repositories
{
    public interface IBucketRepository
    {
      public Task<string> UploadFileAsync(IFormFile file, string destinationPath);
    }
}
