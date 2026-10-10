using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Repositories.Interfaces
{
    public interface IBudgetRepository
    {
        Task<Budget?> GetByIdAsync(Guid id);

        Task<IEnumerable<Budget>> GetByUserIdAsync(Guid userId);

        Task<IEnumerable<Budget>> GetByUserIdAndPeriodAsync(Guid userId, int month, int year);

        Task AddAsync(Budget budget);

        void Update(Budget budget);

        void Delete(Budget budget);

        Task SaveChangesAsync();
    }
}
