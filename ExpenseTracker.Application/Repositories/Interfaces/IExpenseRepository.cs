using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Expense?> GetByIdAsync(Guid id);
        Task<IEnumerable<Expense>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<(Guid CategoryId, string CategoryName, decimal TotalSpent)>> GetTotalsByCategoryAndPeriodAsync(Guid userId, int month, int year);
        Task AddAsync(Expense expense);
        void Update(Expense expense);
        void Delete(Expense expense);
        Task SaveChangesAsync();
    }
}
