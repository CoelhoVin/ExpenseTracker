using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.Budgets
{
    public class BudgetResponse
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}
