using EncryptionProject.Logic.Interfaces;

namespace EncryptionProject.Logic
{
    public enum EncryptionNames { Caesar, Vigenere, Atbash }

    public class EncrypFactory
    {
        private readonly IEnumerable<ICoding> codings;

        public EncrypFactory(IEnumerable<ICoding> _codings)
        {
            codings = _codings;
        }

        public ICoding Get(EncryptionNames name)
        {
            return codings.First(c => c.Name == name);
        }
    }
}
