using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("employees")]
    public class Employee : IIdentifiable
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
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        [Column("language")]
        public string? Language { get; set; }

        [Column("role")]
        public EmployeeRole Role { get; set; }

        [Column("professional_level")]
        public ProfessionalLevel ProfessionalLevel { get; set; }

        [Column("user_id")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }

        [Column("company_id")]
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<EmployeeSkill> Skills { get; set; } = [];
        public ICollection<TaskAssignment> TaskAssignments { get; set; } = [];
        public ICollection<ProjectMember> Projects { get; set; } = [];

        public Guid GetId() => Id;
    }
}