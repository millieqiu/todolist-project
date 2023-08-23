
namespace todoAPP.Services
{
    public class AvatarService
    {
        private string basePath = "./Avatar/";

        public Stream? ReadFile(string fileName)
        {
            string filePath = Path.Combine(basePath, fileName);
            if (File.Exists(filePath) == false)
            {
                return null;
            }

            return new FileStream(filePath, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite, 4096, FileOptions.SequentialScan);
        }

        async public Task<string> WriteFile(IFormFile file)
        {
            var fileOriginName = Path.GetFileName(file.FileName);
            var fileExt = Path.GetExtension(fileOriginName);
            var fileNewName = Path.GetRandomFileName();
            var filePath = Path.Combine(basePath, fileNewName+fileExt);
            if (Directory.Exists(basePath) == false)
            {
                Directory.CreateDirectory(basePath);
            }
            Stream stream = File.Create(filePath);
            await file.CopyToAsync(stream);
            stream.Close();
            return fileNewName + fileExt;

        }

        public void RemoveFile(string fileName)
        {
            string filePath = Path.Combine(basePath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}

