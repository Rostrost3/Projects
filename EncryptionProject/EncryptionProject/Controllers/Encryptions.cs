using EncryptionProject.Logic;
using EncryptionProject.Logic.Algorithms;
using EncryptionProject.Logic.Interfaces;
using EncryptionProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace EncryptionProject.Controllers
{
    public class Encryptions : Controller
    {
        private readonly EncrypFactory factory;

        public Encryptions(EncrypFactory _factory)
        {
            factory = _factory;
        }

        public IActionResult Result()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Caesar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Caesar(CaesarModel info)
        {
            if(ModelState.IsValid)
            {
                string res = "";
                var caesar = factory.Get(EncryptionNames.Caesar);
                if (info.IsEncryp)
                {
                    res = caesar.Encryption(info);
                }
                else
                {
                    res = caesar.Decryption(info);
                }
                return View("Result", res);
            }
            return View(info);
        }

        [HttpGet]
        public IActionResult Vigenere()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Vigenere(VigenereModel info)
        {
            if (ModelState.IsValid)
            {
                string res = "";
                var vigenere = factory.Get(EncryptionNames.Vigenere);
                if (info.IsEncryp)
                {
                    res = vigenere.Encryption(info);
                }
                else
                {
                    res = vigenere.Decryption(info);
                }
                return View("Result", res);
            }
            return View(info);
        }

        [HttpGet]
        public IActionResult Atbash()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Atbash(AtbashModel info)
        {
            if (ModelState.IsValid)
            {
                string res = "";
                var atbash = factory.Get(EncryptionNames.Atbash);
                if (info.IsEncryp)
                {
                    res = atbash.Encryption(info);
                }
                else
                {
                    res = atbash.Decryption(info);
                }
                return View("Result", res);
            }
            return View(info);
        }
    }
}
