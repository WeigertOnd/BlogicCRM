using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogicCRM.Models;

namespace BlogicCRM.ViewModels
{
    public class ContractFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Evidenční číslo je povinné")]
        [StringLength(100, ErrorMessage = "Evidenční číslo nesmí být delší než {1} znaků")]
        [Display(Name = "Evidenční číslo")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instituce je povinná")]
        [StringLength(200, ErrorMessage = "Instituce nesmí být delší než {1} znaků")]
        [Display(Name = "Instituce")]
        public string Institution { get; set; } = string.Empty;

        [Required(ErrorMessage = "Klient je povinný")]
        [Display(Name = "Klient")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Správce smlouvy je povinný")]
        [Display(Name = "Správce smlouvy")]
        public int ManagerAdvisorId { get; set; }

        [Display(Name = "Účastníci")]
        public List<int> SelectedAdvisorIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Datum uzavření je povinné")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum uzavření")]
        public DateTime DateClosed { get; set; }

        [Required(ErrorMessage = "Datum platnosti je povinné")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum platnosti")]
        public DateTime DateValidFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum ukončení")]
        public DateTime? DateEnded { get; set; }

        // Lists for dropdowns/checkboxes
        // FullName projection for display is handled in the controller (avoid changing models).
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Advisor> Advisors { get; set; } = new List<Advisor>();
    }
}
