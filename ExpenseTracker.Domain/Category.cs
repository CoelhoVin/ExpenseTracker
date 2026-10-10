namespace ExpenseTracker.Domain
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    }
}