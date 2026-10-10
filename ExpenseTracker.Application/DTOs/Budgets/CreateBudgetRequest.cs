using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.Budgets
{
    public class CreateBudgetRequest
    {
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }

        [Range(2000, 2100)]
        public int Year { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
