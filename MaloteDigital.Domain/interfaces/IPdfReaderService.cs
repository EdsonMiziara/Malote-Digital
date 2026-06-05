namespace MaloteDigital.Domain.interfaces;

public interface IPdfReaderService
{
    /// <summary>
    /// opens a local pdf and extracts all text content, returning it as a string.
    /// </summary>
    Task<string> ExtractTextAsync(string filePath);
}
