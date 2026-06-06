using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Jméno je povinné.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Jméno musí mít alespoň 2 znaky.")]
        [RegularExpression(@"^(?=.*[A-Za-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽáčďéěíňóřšťúůýž])[A-Za-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽáčďéěíňóřšťúůýž -]+$", ErrorMessage = "Jméno může obsahovat pouze písmena, mezery nebo pomlčku.")]
        [Display(Name = "Jméno")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Příjmení je povinné.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Příjmení musí mít alespoň 2 znaky.")]
        [RegularExpression(@"^(?=.*[A-Za-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽáčďéěíňóřšťúůýž])[A-Za-zÁČĎÉĚÍŇÓŘŠŤÚŮÝŽáčďéěíňóřšťúůýž -]+$", ErrorMessage = "Příjmení může obsahovat pouze písmena, mezery nebo pomlčku.")]
        [Display(Name = "Příjmení")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou e-mailovou adresu.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$", ErrorMessage = "Zadejte platnou e-mailovou adresu ve formátu např. uzivatel@domena.cz.")]
        [StringLength(200, ErrorMessage = "E-mail nesmí být delší než {1} znaků")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon je povinný.")]
        [RegularExpression(@"^\+420\s?[0-9]{3}\s?[0-9]{3}\s?[0-9]{3}$", ErrorMessage = "Telefon musí být ve formátu +420123456789 nebo +420 123 456 789.")]
        [StringLength(50, ErrorMessage = "Telefon nesmí být delší než {1} znaků")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rodné číslo je povinné.")]
        [RegularExpression(@"^\d{6}/\d{3,4}$", ErrorMessage = "Rodné číslo musí být ve formátu 800101/1234.")]
        [StringLength(20, ErrorMessage = "Rodné číslo nesmí být delší než {1} znaků")]
        [Display(Name = "Rodné číslo")]
        public string BirthNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Věk je povinný.")]
        [Range(18, 100, ErrorMessage = "Věk musí být v rozmezí 18 až 100 let.")]
        [Display(Name = "Věk")]
        public int Age { get; set; }

        [Display(Name = "Smlouvy")]
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
