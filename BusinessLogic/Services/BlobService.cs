namespace BusinessLogic.Services;

using System;
using System.IO;
using System.Threading.Tasks;

public class BlobService
{
    private readonly string _uploadFolder;

    public BlobService(string rootPath)
    {
        _uploadFolder = Path.Combine(rootPath, "Upload");

        if (!Directory.Exists(_uploadFolder))
        {
            Directory.CreateDirectory(_uploadFolder);
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        if (fileStream is null || fileStream.Length is 0)
        {
            throw new ArgumentException("File stream is empty or null.");
        }

        var destinationPath = Path.Combine(_uploadFolder, fileName);

        await using (var stream = new FileStream(destinationPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        return GetFileUrl(fileName);
    }

    public void DeleteFile(string fileName)
    {
        var filePath = Path.Combine(_uploadFolder, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private static string GetFileUrl(string fileName)
    {
        return $"/upload/{fileName}";
    }
}
