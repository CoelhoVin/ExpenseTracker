using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.Categories
{
    public class UpdateCategoryRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
