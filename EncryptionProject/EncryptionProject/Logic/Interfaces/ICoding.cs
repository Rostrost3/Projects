namespace EncryptionProject.Logic.Interfaces
{
    public interface ICoding
    {
        EncryptionNames Name { get; }
        string Encryption(EncryptionOptions options);
        string Decryption(EncryptionOptions options);
    }
}
