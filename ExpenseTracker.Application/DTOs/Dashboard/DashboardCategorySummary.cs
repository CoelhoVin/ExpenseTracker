using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.Dashboard
{
    public class DashboardCategorySummary
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Budget { get; set; }

        public decimal Spent { get; set; }

        public decimal Remaining { get; set; }
    }
}
