using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ExpenseRepository(ApplicationDbContext context) {
            _context = context;
        }


        public async Task AddAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
        }

        public void Delete(Expense expense)
        {
            _context.Expenses.Remove(expense);
        }

        public async Task<Expense?> GetByIdAsync(Guid id)
        {
            return await _context.Expenses.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Expense>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Expenses
                                 .Include(e => e.Category)
                                 .Where(e => e.UserId == userId)
                                 .OrderByDescending(e => e.Date)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<(Guid CategoryId, string CategoryName, decimal TotalSpent)>> GetTotalsByCategoryAndPeriodAsync(Guid userId, int month, int year)
        {
            var results = await _context.Expenses.Where(e =>
                                                        e.UserId == userId && 
                                                        e.Date.Month == month &&
                                                        e.Date.Year == year)
                                                 .GroupBy(e => new
                                                 {
                                                     e.CategoryId,
                                                     CategoryName = e.Category.Name
                                                 })
                                                 .Select(g => new
                                                        {
                                                            CategoryId = g.Key.CategoryId,
                                                            CategoryName = g.Key.CategoryName,
                                                            TotalSpent = g.Sum(e => e.Amount)
                                                        }).ToListAsync();

            return results.Select(x => (x.CategoryId, x.CategoryName, x.TotalSpent));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Expense expense)
        {
            _context.Expenses.Update(expense);
        }
    }
}
