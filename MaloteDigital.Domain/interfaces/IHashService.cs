namespace MaloteDigital.Domain.interfaces;

public interface IHashService
{
    ///<summary>
    /// generates a hash string for the given stream, which can be used to identify the file and check for duplicates.
    /// </summary>
    string ComputeHash(Stream stream);
}
