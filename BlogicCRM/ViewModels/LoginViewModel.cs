using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou e-mailovou adresu.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo je povinné.")]
        public string Password { get; set; } = string.Empty;
    }
}
