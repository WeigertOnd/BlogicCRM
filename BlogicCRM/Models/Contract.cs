using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogicCRM.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Evidenční číslo je povinné")]
        [StringLength(100, ErrorMessage = "Evidenční číslo nesmí být delší než {1} znaků")]
        [Display(Name = "Evidenční číslo")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instituce je povinná")]
        [StringLength(200, ErrorMessage = "Instituce nesmí být delší než {1} znaků")]
        [Display(Name = "Instituce")]
        public string Institution { get; set; } = string.Empty;

        [Display(Name = "Klient")]
        [Range(1, int.MaxValue, ErrorMessage = "Vyberte klienta")]
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [Display(Name = "Správce smlouvy")]
        [Range(1, int.MaxValue, ErrorMessage = "Vyberte správce smlouvy")]
        public int ManagerAdvisorId { get; set; }
        public Advisor? ManagerAdvisor { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum uzavření")]
        public DateTime DateClosed { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum platnosti")]
        public DateTime DateValidFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum ukončení")]
        public DateTime? DateEnded { get; set; }

        public ICollection<ContractAdvisor>? ContractAdvisors { get; set; }
    }
}
