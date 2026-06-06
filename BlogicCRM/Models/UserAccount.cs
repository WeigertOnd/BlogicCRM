using System;
using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "E-mail je povinný.")]
        [EmailAddress(ErrorMessage = "Zadejte platnou e-mailovou adresu.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$", ErrorMessage = "Zadejte platnou e-mailovou adresu ve formátu např. uzivatel@domena.cz.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
