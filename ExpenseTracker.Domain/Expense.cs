namespace ExpenseTracker.Domain
{
    public class Expense
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}