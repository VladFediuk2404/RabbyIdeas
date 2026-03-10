using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("companies")]
    public class Company : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("invite_key")]
        public string InviteKey { get; set; } = Guid.NewGuid().ToString("N")[..12].ToUpper();

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("owner_id")]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<User> Members { get; set; } = [];
        public ICollection<Employee> Employees { get; set; } = [];
        public ICollection<Project> Projects { get; set; } = [];

        public Guid GetId() => Id;
        protected Company() { }
    }
}