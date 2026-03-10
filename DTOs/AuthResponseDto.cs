namespace Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public Guid? CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }
}