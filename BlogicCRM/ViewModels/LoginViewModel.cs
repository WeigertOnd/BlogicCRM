using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou e-mailovou adresu.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$", ErrorMessage = "Zadejte platnou e-mailovou adresu ve formátu např. uzivatel@domena.cz.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo je povinné.")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = string.Empty;
    }
}
