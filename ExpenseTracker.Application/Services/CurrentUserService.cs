using ExpenseTracker.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ExpenseTracker.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (string.IsNullOrWhiteSpace(userId)) {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }
                return Guid.Parse(userId);
            }
        } 
    }
}
