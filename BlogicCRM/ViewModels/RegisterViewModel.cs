using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou e-mailovou adresu.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$", ErrorMessage = "Zadejte platnou e-mailovou adresu ve formátu např. uzivatel@domena.cz.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrzení e-mailu je povinné.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$", ErrorMessage = "Zadejte platnou e-mailovou adresu ve formátu např. uzivatel@domena.cz.")]
        [Compare("Email", ErrorMessage = "E-maily se neshodují.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Potvrzení e-mailu")]
        public string ConfirmEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo je povinné.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{10,}$", ErrorMessage = "Heslo musí mít alespoň 10 znaků, obsahovat alespoň jedno velké písmeno a alespoň jedno číslo.")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrzení hesla")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
