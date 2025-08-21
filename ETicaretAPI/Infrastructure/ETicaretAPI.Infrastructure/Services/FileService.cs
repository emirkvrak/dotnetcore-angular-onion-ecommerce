using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();

            List<bool> results = new();

            foreach (var file in files)
            {

                string fileNewName = await FileRenameAsync(uploadPath, file.FileName);

                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);

                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));

                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
            {
                return datas;
            }

            return null;
        }


        private async Task<string> FileRenameAsync(string path, string fileName)
        {
            return await Task.Run(() =>
            {
                string extension = Path.GetExtension(fileName);
                string onlyFileName = Path.GetFileNameWithoutExtension(fileName);

                
                string baseName = NameOperation.CharacterRegulatory(onlyFileName);

                
                string newFileName = baseName + extension;
                int fileCounter = 1;

                
                while (File.Exists(Path.Combine(path, newFileName)))
                {
                    newFileName = $"{baseName}-{fileCounter}{extension}";
                    fileCounter++;
                }

                return newFileName;
            });
        }

    }

}



