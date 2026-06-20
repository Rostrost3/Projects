using EncryptionProject.Logic.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EncryptionProject.Models
{
    public class AtbashModel : EncryptionOptions
    {
        [Display(Description = "Текст")]
        [Required(ErrorMessage = "Нельзя закодировать/декодировать пустое сообщение")]
        public string Text { get; set; }

        [Display(Description = "Вы хотите зашифровать(да) или расшифровать(нет) сообщение?")]
        public bool IsEncryp { get; set; }
    }
}
