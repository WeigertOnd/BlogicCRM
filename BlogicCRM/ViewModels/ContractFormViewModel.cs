using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogicCRM.Models;

namespace BlogicCRM.ViewModels
{
    public class ContractFormViewModel : IValidatableObject
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Evidenční číslo je povinné.")]
        [RegularExpression(@"^C-\d{4}-\d{3}$", ErrorMessage = "Evidenční číslo musí být ve formátu C-2026-001.")]
        [StringLength(100, ErrorMessage = "Evidenční číslo nesmí být delší než {1} znaků")]
        [Display(Name = "Evidenční číslo")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instituce je povinná.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Název instituce musí mít alespoň 2 znaky.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "Instituce je povinná.")]
        [Display(Name = "Instituce")]
        public string Institution { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vyberte klienta.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vyberte klienta.")]
        [Display(Name = "Klient")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Vyberte správce smlouvy.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vyberte správce smlouvy.")]
        [Display(Name = "Správce smlouvy")]
        public int ManagerAdvisorId { get; set; }

        [Display(Name = "Další účastníci smlouvy")]
        public List<int> SelectedAdvisorIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Datum uzavření je povinné.")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum uzavření")]
        public DateTime? DateClosed { get; set; }

        [Required(ErrorMessage = "Datum platnosti je povinné.")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum platnosti")]
        public DateTime? DateValidFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum ukončení")]
        public DateTime? DateEnded { get; set; }

        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Advisor> Advisors { get; set; } = new List<Advisor>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateClosed.HasValue && DateValidFrom.HasValue && DateValidFrom.Value < DateClosed.Value)
            {
                yield return new ValidationResult(
                    "Datum platnosti nesmí být dříve než datum uzavření.",
                    new[] { nameof(DateValidFrom) });
            }

            if (DateClosed.HasValue && DateEnded.HasValue && DateEnded.Value < DateClosed.Value)
            {
                yield return new ValidationResult(
                    "Datum ukončení nesmí být dříve než datum uzavření.",
                    new[] { nameof(DateEnded) });
            }
        }
    }
}
