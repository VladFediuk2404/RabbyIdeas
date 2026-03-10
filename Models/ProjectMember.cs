using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("project_members")]
    public class ProjectMember : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [Column("project_id")]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Column("joined_at")]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public Guid GetId() => Id;
    }
}