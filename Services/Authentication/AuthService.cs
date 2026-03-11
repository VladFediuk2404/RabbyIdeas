using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data;
using Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<User> userManager, AppDbContext context, IConfiguration config, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        // Step 1: Register → send OTP
        public async Task RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            try
            {
                await SendOtpAsync(dto.Email);
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw new Exception("Failed to send confirmation email. Please try again.");
            }
        }

        // Step 2: Confirm OTP → return JWT
        public async Task<AuthResponseDto> ConfirmOtpAsync(ConfirmOtpDto dto)
        {
            var otp = await _context.OtpCodes
                .Where(o => o.Email == dto.Email && o.Code == dto.Code && !o.IsUsed)
                .FirstOrDefaultAsync();

            if (otp == null || otp.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired code.");

            otp.IsUsed = true;
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByEmailAsync(dto.Email)
                ?? throw new Exception("User not found.");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            var company = user.CompanyId.HasValue
                ? await _context.Companies.FirstOrDefaultAsync(c => c.Id == user.CompanyId)
                : null;

            return BuildResponse(user, company);
        }

        // Login → send OTP
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid email or password.");

            if (!user.EmailConfirmed)
                throw new Exception("Email not confirmed. Please check your email for the confirmation code.");

            await SendOtpAsync(dto.Email);

            return new AuthResponseDto
            {
                Token = null!,
                UserName = user.UserName!,
                UserId = user.Id,
                CompanyId = user.CompanyId,
                CompanyName = null
            };
        }

        private async Task SendOtpAsync(string email)
        {
            var oldCodes = _context.OtpCodes.Where(o => o.Email == email && !o.IsUsed);
            _context.OtpCodes.RemoveRange(oldCodes);

            var code = new Random().Next(100000, 999999).ToString();
            _context.OtpCodes.Add(new OtpCode { Email = email, Code = code });
            await _context.SaveChangesAsync();

            await _emailService.SendOtpAsync(email, code);
        }

        private AuthResponseDto BuildResponse(User user, Company? company) => new()
        {
            Token = GenerateToken(user),
            UserName = user.UserName!,
            UserId = user.Id,
            CompanyId = company?.Id,
            CompanyName = company?.Name
        };

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:ExpiryDays"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}