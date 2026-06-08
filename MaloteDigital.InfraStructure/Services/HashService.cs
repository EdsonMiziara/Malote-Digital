using MaloteDigital.Domain.interfaces;
using System.Security.Cryptography;

namespace MaloteDigital.InfraStructure.Services;

public class HashService : IHashService
{
    public string ComputeHash(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream), "O fluxo de dados não pode ser nulo");

        byte[] hashBytes = SHA256.HashData(stream);
        return Convert.ToHexString(hashBytes).ToLower();

    }
}
