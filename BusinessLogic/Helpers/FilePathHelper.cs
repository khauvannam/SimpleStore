namespace BusinessLogic.Helpers;

public static class FilePathHelper
{
    public static string CreateFilePath(string baseDirectory, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        // Generate the full file path for the file storage
        return Path.Combine(baseDirectory, fileName);
    }

    public static string GetFileUrl(string fileName)
    {
        // Generate the relative URL to access the uploaded file
        return $"/uploads/{fileName}";
    }
}
