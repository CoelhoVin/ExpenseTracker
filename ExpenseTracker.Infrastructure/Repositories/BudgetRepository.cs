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
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Budget?> GetByIdAsync(Guid id)
        {
            return await _context.Budgets
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Budget>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Year)
                .ThenByDescending(b => b.Month)
                .ToListAsync();
        }

        public async Task AddAsync(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
        }

        public void Update(Budget budget)
        {
            _context.Budgets.Update(budget);
        }

        public void Delete(Budget budget)
        {
            _context.Budgets.Remove(budget);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Budget>> GetByUserIdAndPeriodAsync(Guid userId, int month, int year)
        {
           return await _context.Budgets.Include(b => b.Category)
                                         .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
                                         .OrderBy(b => b.Category.Name).ToListAsync();
        }
    }
}
