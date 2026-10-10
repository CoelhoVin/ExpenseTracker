using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
