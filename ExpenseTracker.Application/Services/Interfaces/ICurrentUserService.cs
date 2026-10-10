using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
