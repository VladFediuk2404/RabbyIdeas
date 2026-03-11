using Dtos;

namespace Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> ConfirmOtpAsync(ConfirmOtpDto dto); 
    }
}