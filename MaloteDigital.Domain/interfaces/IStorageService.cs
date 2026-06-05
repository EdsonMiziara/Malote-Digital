namespace MaloteDigital.Domain.interfaces;

public interface IStorageService
{
    /// <summary> 
    /// Uploads a file to the storage service and returns the URL of the uploaded file.
    /// </summary>
    /// <param name="fileStream">The stream of the file to be uploaded.</param>
    /// <param name="fileName"> The original or desired name to the file</param>
    /// <param name="subFolder">The destination folder</param>
    
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string subFolder);

}
