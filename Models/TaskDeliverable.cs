using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("task_deliverables")]
    public class TaskDeliverable : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [Column("task_id")]
        public Guid TaskId { get; set; }
        public MyTask Task { get; set; }

        [Required]
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("title")]
        public string Title { get; set; }

        [MaxLength(500)]
        [Column("description")]
        public string? Description { get; set; }

        [Column("type")]
        public DeliverableType Type { get; set; }

        [MaxLength(500)]
        [Column("file_url")]
        public string? FileUrl { get; set; }

        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [Column("status")]
        public DeliverableStatus Status { get; set; } = DeliverableStatus.Submitted;

        public Guid GetId() => Id;
    }
}