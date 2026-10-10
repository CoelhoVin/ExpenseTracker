using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId);
        Task<Category?> GetByIdAndUserIdAsync(Guid id, Guid userId);

        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
        Task SaveChangesAsync();
    }
}
