using MaloteDigital.Domain.interfaces;

namespace MaloteDigital.InfraStructure.Services;

public class LocalStorageService : IStorageService
{
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string subFolder)
    {
        if (fileStream == null || fileName.Length == 0)
        {
            throw new ArgumentException("O fluxo do arquivo está vazio ou invalido");
        }
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);

        if (Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.CreateVersion7()}{extension}";

        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var destinationFolder = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await fileStream.CopyToAsync(destinationFolder);
        }

        return $"/uploads/{subFolder}/{uniqueFileName}";
    }
}
