using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.Dashboard
{
    public class DashboardSummaryResponse
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public decimal TotalBudget { get; set; }

        public decimal TotalSpent { get; set; }

        public decimal Remaining { get; set; }

        public IEnumerable<DashboardCategorySummary> Categories { get; set; } = new List<DashboardCategorySummary>();
    }
}
