namespace ExpenseTracker.Domain
{
    public class Budget
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}