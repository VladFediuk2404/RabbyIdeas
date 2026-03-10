using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("projects")]
    public class Project : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; }

        [MaxLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        [Column("status")]
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Column("manager_id")]
        public Guid ManagerId { get; set; }
        public Employee Manager { get; set; }

        [Column("scrum_master_id")]
        public Guid? ScrumMasterId { get; set; }
        public Employee? ScrumMaster { get; set; }

        [Column("client_report_id")]
        public Guid? ClientReportId { get; set; }
        public ClientReport? ClientReport { get; set; }

        [Column("company_id")]
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }

        public ICollection<MyTask> Tasks { get; set; } = [];
        public ICollection<ProjectMember> Members { get; set; } = [];

        public Guid GetId() => Id;
    }
}