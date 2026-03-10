using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("task_commits")]
    public class TaskCommit : IIdentifiable
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
        [MaxLength(500)]
        [Column("message")]
        public string Message { get; set; }

        [MaxLength(200)]
        [Column("branch")]
        public string? Branch { get; set; }

        [MaxLength(100)]
        [Column("commit_hash")]
        public string? CommitHash { get; set; }

        [Column("committed_at")]
        public DateTime CommittedAt { get; set; } = DateTime.UtcNow;

        public Guid GetId() => Id;
    }
}