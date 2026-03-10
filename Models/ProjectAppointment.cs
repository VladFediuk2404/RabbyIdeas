using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("project_appointments")]
    public class ProjectAppointment : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [Column("project_id")]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        [Column("scrum_master_id")]
        public Guid ScrumMasterId { get; set; }
        public Employee ScrumMaster { get; set; }

        [Required]
        [Column("appointed_by")]
        public Guid AppointedById { get; set; }
        public Employee AppointedBy { get; set; }

        [Column("appointed_at")]
        public DateTime AppointedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        [Column("notes")]
        public string? Notes { get; set; }

        public Guid GetId() => Id;
    }
}