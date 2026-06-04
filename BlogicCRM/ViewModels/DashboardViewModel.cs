using System.Collections.Generic;
using BlogicCRM.Models;

namespace BlogicCRM.ViewModels
{
    public class DashboardViewModel
    {
        public int ClientsCount { get; set; }
        public int AdvisorsCount { get; set; }
        public int ContractsCount { get; set; }
        public int ActiveContractsCount { get; set; }
        public int EndedContractsCount { get; set; }

        public List<Contract> RecentContracts { get; set; } = new List<Contract>();
    }
}
