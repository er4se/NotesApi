using Microsoft.AspNetCore.Identity;
using NotesApi.Application.Common;
using NotesApi.Application.Interfaces;
using NotesApi.Domain.Common.Exceptions;
using NotesApi.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public IdentityService(
            UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Result<Guid>> RegisterAsync(string email, string password)
        {
            if (await UserExistsAsync(email))
                throw new ConflictException("User with this email already exists");

            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            return result.Succeeded
                ? Result<Guid>.Success(user.Id)
                : Result<Guid>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Result<string>.Failure("Invalid credentials");

            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isValid) return Result<string>.Failure("Invalid credentials");

            var token = _jwtTokenService.GenerateToken(user.Id, user.Email!);
            return Result<string>.Success(token);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
