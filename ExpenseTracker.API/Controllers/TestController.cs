using ExpenseTracker.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public TestController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [Authorize]
        [HttpGet("private")]
        public IActionResult Private()
        {
            return Ok(new
            {
                message = "You are authenticated!",
                userId = _currentUserService.UserId
            });
        }
    }
}
