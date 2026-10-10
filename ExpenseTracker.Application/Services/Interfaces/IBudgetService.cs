using ExpenseTracker.Application.DTOs.Budgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetResponse> CreateAsync(CreateBudgetRequest request);
        Task<IEnumerable<BudgetResponse>> GetAllAsync();
        Task<BudgetResponse> GetByIdAsync(Guid id);
        Task<BudgetResponse> UpdateAsync(Guid id, UpdateBudgetRequest request);
        Task DeleteAsync(Guid id);
    }
}
