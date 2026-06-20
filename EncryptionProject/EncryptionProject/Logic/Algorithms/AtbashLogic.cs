using EncryptionProject.Logic.Interfaces;
using EncryptionProject.Models;
using Microsoft.Extensions.Options;

namespace EncryptionProject.Logic.Algorithms
{
    public class AtbashLogic : ICoding
    {
        private readonly MySettings mySettings;

        public AtbashLogic(IOptions<MySettings> _mysettings)
        {
            mySettings = _mysettings.Value;
        }

        public EncryptionNames Name => EncryptionNames.Atbash;
        public string Encryption(EncryptionOptions options)
        {
            var atbash = options as AtbashModel ?? throw new ArgumentException("Invalid options for Atbash");
            atbash.Text = atbash.Text.ToLower();
            string res = "";
            foreach (var elem in atbash.Text)
            {
                if (mySettings.SpecialSymbols.Contains(elem))
                {
                    res += elem;
                }
                else
                {
                    res += mySettings.RussianAlphabet[mySettings.RussianAlphabet.Length - mySettings.RussianAlphabet.IndexOf(elem) - 1];
                }
            }
            return res;
        }

        public string Decryption(EncryptionOptions options)
        {
            var atbash = options as AtbashModel ?? throw new ArgumentException("Invalid options for Atbash");
            atbash.Text = atbash.Text.ToLower();
            string res = "";
            foreach(var elem in atbash.Text)
            {
                if (mySettings.SpecialSymbols.Contains(elem))
                {
                    res += elem;
                }
                else
                {
                    res += mySettings.RussianAlphabet[mySettings.RussianAlphabet.Length - mySettings.RussianAlphabet.IndexOf(elem)];
                }
            }
            return res;
        }

    }
}
