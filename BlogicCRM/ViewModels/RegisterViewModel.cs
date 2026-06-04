using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou e-mailovou adresu.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrzení e-mailu je povinné.")]
        [Compare("Email", ErrorMessage = "E-maily se neshodují.")]
        public string ConfirmEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo je povinné.")]
        [MinLength(10, ErrorMessage = "Heslo musí mít alespoň 10 znaků.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
