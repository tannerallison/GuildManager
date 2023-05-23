using System.Security.Cryptography;

namespace GuildManager.Utilities;

/// <summary>
/// Pulled from http://csharptest.net/470/another-example-of-how-to-store-a-salted-password-hash/
/// </summary>
public sealed class PasswordHash
{
    const int SaltSize = 16, HashSize = 20, HashIter = 10000;
    readonly byte[] _salt, _hash;

    public PasswordHash(string password)
    {
        _salt = RandomNumberGenerator.GetBytes(SaltSize);
        _hash = GetHashedBytes(password);
    }

    private byte[] GetHashedBytes(string password) =>
        new Rfc2898DeriveBytes(password, _salt, HashIter, HashAlgorithmName.SHA512).GetBytes(HashSize);


    public PasswordHash(byte[] hashBytes)
    {
        Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
        Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
    }

    public PasswordHash(byte[] salt, byte[] hash)
    {
        Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
        Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
    }

    public byte[] ToArray()
    {
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
        return hashBytes;
    }

    public byte[] Salt
    {
        get { return (byte[])_salt.Clone(); }
    }

    public byte[] Hash
    {
        get { return (byte[])_hash.Clone(); }
    }

    public bool Verify(string password)
    {
        byte[] test = GetHashedBytes(password);
        for (int i = 0; i < HashSize; i++)
            if (test[i] != _hash[i])
                return false;
        return true;
    }
}
