using ExpenseTracker.Application.DTOs.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseResponse> CreateAsync(CreateExpenseRequest request);
        Task<IEnumerable<ExpenseResponse>> GetAllAsync();
        Task<ExpenseResponse> GetByIdAsync(Guid id);
        Task<ExpenseResponse> UpdateAsync(Guid id, UpdateExpenseRequest request);
        Task DeleteAsync(Guid id);
    }
}
