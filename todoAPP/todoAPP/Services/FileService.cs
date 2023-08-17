using System;
using Microsoft.Extensions.FileProviders;

namespace todoAPP.Services
{
	public class FileService
	{
        public byte[]? ReadFile(string fileName)
        {
            if(File.Exists("./Avatar/" + fileName) == false)
            {
                return null;
            }

            return File.ReadAllBytes("./Avatar/"+ fileName);
        }

        async public Task<string> WriteFile(IFormFile file)
		{
            var fileOriginName = Path.GetFileName(file.FileName);
            var fileExt = Path.GetExtension(fileOriginName);
            var fileNewName = Path.GetRandomFileName();
            var filePath = "./Avatar/" + fileNewName + fileExt;
            if (Directory.Exists("./Avatar/") == false)
            {
                Directory.CreateDirectory("./Avatar/");
            }
            Stream stream = File.Create(filePath);
            await file.CopyToAsync(stream);
            stream.Close();
            return fileNewName + fileExt;

        }

        public void RemoveFile(string fileName)
        {
            string filePath = "./Avatar/" + fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
	}
}

