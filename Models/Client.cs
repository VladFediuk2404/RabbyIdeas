using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("clients")]
    public class Client : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        [Column("email")]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        [Column("phone")]
        public string? Phone { get; set; }

        [MaxLength(200)]
        [Column("company")]
        public string? Company { get; set; }

        [MaxLength(500)]
        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ClientReport> Reports { get; set; } = [];

        public Guid GetId() => Id;
    }
}