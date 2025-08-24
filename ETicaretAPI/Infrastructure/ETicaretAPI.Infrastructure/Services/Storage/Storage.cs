using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class Storage 
    {

        protected delegate bool HasFile(string pathOrContainerNaem, string fileName);

        protected async Task<string> FileRenameAsync(string pathOrContainerNaem, string fileName, HasFile hasFileMethod, bool first = true)
        {
            return await Task.Run(() =>
            {
                string extension = Path.GetExtension(fileName);
                string onlyFileName = Path.GetFileNameWithoutExtension(fileName);


                string baseName = NameOperation.CharacterRegulatory(onlyFileName);


                string newFileName = baseName + extension;
                int fileCounter = 1;


                while (hasFileMethod(pathOrContainerNaem, newFileName))
                {
                    newFileName = $"{baseName}-{fileCounter}{extension}";
                    fileCounter++;
                }

                return newFileName;
            });
        }
    }
}
