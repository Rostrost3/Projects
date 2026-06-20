using EncryptionProject.Logic.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EncryptionProject.Models
{
    public class CaesarModel : EncryptionOptions
    {
        [Display(Description = "Текст")]
        [Required(ErrorMessage = "Нельзя закодировать/декодировать пустое сообщение")]
        public string Text { get; set; }

        [Display(Description = "Сдвиг")]
        [Required(ErrorMessage = "Сдвиг не может быть пустым")]
        [Range(0, 35, ErrorMessage = "Сдвиг не может быть отрицательным, либо слишком большим")]
        public int? Shift { get; set; }

        [Display(Description = "Вы хотите зашифровать(да) или расшифровать(нет) сообщение?")]
        public bool IsEncryp { get; set; }
    }
}
