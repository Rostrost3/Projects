using EncryptionProject.Logic.Interfaces;
using EncryptionProject.Models;
using Microsoft.Extensions.Options;

namespace EncryptionProject.Logic.Algorithms
{
    public class CaesarLogic : ICoding
    {
        private readonly MySettings mySettings;

        public CaesarLogic(IOptions<MySettings> settings)
        {
            mySettings = settings.Value;
        }

        public EncryptionNames Name => EncryptionNames.Caesar;

        public string Encryption(EncryptionOptions options)
        {
            var caesar = options as CaesarModel ?? throw new ArgumentException("Invalid options for Caesar");
            var text = caesar.Text.ToLower();
            if(caesar.Shift == 0)
            {
                return text;
            }
            string res = "";
            int textInd = 0;
            while(textInd != text.Length)
            {
                if (mySettings.SpecialSymbols.IndexOf(text[textInd]) != -1)
                {
                    res += text[textInd];
                    ++textInd;
                    continue;
                }
                int index = mySettings.RussianAlphabet.IndexOf(text[textInd]);
                index += (int)caesar.Shift;
                while(index >= mySettings.RussianAlphabet.Length)
                {
                    index -= mySettings.RussianAlphabet.Length;
                }
                res += mySettings.RussianAlphabet[index];
                ++textInd;
            }
            return res;
        }

        public string Decryption(EncryptionOptions options)
        {
            var caesar = options as CaesarModel ?? throw new ArgumentException("Invalid options for Caesar");
            var text = caesar.Text.ToLower();
            if (caesar.Shift == 0)
            {
                return text;
            }
            string res = "";
            int textInd = 0;
            while (textInd != text.Length)
            {
                if (mySettings.SpecialSymbols.IndexOf(text[textInd]) != -1)
                {
                    res += text[textInd];
                    ++textInd;
                    continue;
                }
                int index = mySettings.RussianAlphabet.IndexOf(text[textInd]);
                index -= (int)caesar.Shift;
                while (index < 0)
                {
                    index = mySettings.RussianAlphabet.Length - Math.Abs(index);
                }
                res += mySettings.RussianAlphabet[index];
                ++textInd;
            }
            return res;
        }
    }
}