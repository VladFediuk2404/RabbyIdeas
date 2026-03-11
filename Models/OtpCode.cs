using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("otp_codes")]
    public class OtpCode
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(10);

        [Column("is_used")]
        public bool IsUsed { get; set; } = false;
    }
}