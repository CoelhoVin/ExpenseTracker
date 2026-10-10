using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.Expenses
{
    public class UpdateExpenseRequest
    {
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
