using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Jméno je povinné")]
        [StringLength(100, ErrorMessage = "Jméno nesmí být delší než {1} znaků")]
        [Display(Name = "Jméno")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Příjmení je povinné")]
        [StringLength(100, ErrorMessage = "Příjmení nesmí být delší než {1} znaků")]
        [Display(Name = "Příjmení")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail je povinný")]
        [EmailAddress(ErrorMessage = "Neplatná e-mailová adresa")]
        [StringLength(200, ErrorMessage = "E-mail nesmí být delší než {1} znaků")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon je povinný")]
        [Phone(ErrorMessage = "Neplatné telefonní číslo")]
        [StringLength(50, ErrorMessage = "Telefon nesmí být delší než {1} znaků")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rodné číslo je povinné")]
        [StringLength(20, ErrorMessage = "Rodné číslo nesmí být delší než {1} znaků")]
        [Display(Name = "Rodné číslo")]
        public string BirthNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Věk je povinný")]
        [Range(0, 150, ErrorMessage = "Věk musí být mezi {1} a {2}")]
        [Display(Name = "Věk")]
        public int Age { get; set; }

        [Display(Name = "Smlouvy")]
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
