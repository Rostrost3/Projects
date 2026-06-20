using EncryptionProject.Logic.Interfaces;
using EncryptionProject.Models;
using Microsoft.Extensions.Options;

namespace EncryptionProject.Logic.Algorithms
{
    public class VigenereLogic : ICoding
    {
        private readonly MySettings mySettings;

        public VigenereLogic(IOptions<MySettings> _mySettings)
        {
            mySettings = _mySettings.Value;
        }

        public EncryptionNames Name => EncryptionNames.Vigenere;

        //TODO чтобы пробелы учитывались
        public string Encryption(EncryptionOptions options)
        {
            var vigenere = options as VigenereModel ?? throw new ArgumentException("Invalid options for Vigenere");
            vigenere.Text = vigenere.Text.ToLower();
            vigenere.Key = vigenere.Key.ToLower();
            string firstStep = "";
            int textLength = vigenere.Text.Length;
            while (textLength > vigenere.Key.Length)
            {
                firstStep += vigenere.Key;
                textLength -= vigenere.Key.Length;
            }
            for (int i = 0; i < textLength; ++i)
            {
                firstStep += vigenere.Key[i];
            }
            string res = "";
            int index = 0;
            while (res.Length != vigenere.Text.Length)
            {
                res += mySettings.RussianAlphabet[(mySettings.RussianAlphabet.IndexOf(vigenere.Text[index]) + mySettings.RussianAlphabet.IndexOf(firstStep[index])) % mySettings.RussianAlphabet.Length];
                ++index;
            }
            return res;
        }

        public string Decryption(EncryptionOptions options)
        {
            var vigenere = options as VigenereModel ?? throw new ArgumentException("Invalid options for Vigenere");
            vigenere.Text = vigenere.Text.ToLower();
            vigenere.Key = vigenere.Key.ToLower();
            string firstStep = "";
            int textLength = vigenere.Text.Length;
            while (textLength > vigenere.Key.Length)
            {
                firstStep += vigenere.Key;
                textLength -= vigenere.Key.Length;
            }
            for (int i = 0; i < textLength; ++i)
            {
                firstStep += vigenere.Key[i];
            }
            string res = "";
            int index = 0;
            while (res.Length != vigenere.Text.Length)
            {
                res += mySettings.RussianAlphabet[FindIndex(mySettings.RussianAlphabet.IndexOf(firstStep[index]), mySettings.RussianAlphabet.IndexOf(vigenere.Text[index]), mySettings.RussianAlphabet.Length)];
                ++index;
            }
            return res;
        }

        private int FindIndex(int key, int text, int length)
        {
            int res = -1;
            int k = 1;
            while(res < 0)
            {
                res = length * k + (text - key);
                ++k;
            }
            return res % length;
        }
    }
}
