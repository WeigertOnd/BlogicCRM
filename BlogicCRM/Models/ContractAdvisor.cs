using System.ComponentModel.DataAnnotations.Schema;

namespace BlogicCRM.Models
{
    public class ContractAdvisor
    {
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        public int AdvisorId { get; set; }
        public Advisor? Advisor { get; set; }
    }
}
