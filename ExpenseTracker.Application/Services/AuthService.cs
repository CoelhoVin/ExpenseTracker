using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository; 
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }


        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            var passwordValid = _passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash);

            if (!passwordValid)
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(
                user.Id,
                user.Name,
                user.Email);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser is not null) {
                throw new InvalidOperationException("Email already registered.");
            }

            var passwordHash = _passwordService.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
