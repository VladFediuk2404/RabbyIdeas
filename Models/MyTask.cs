using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tasks")]
    public class MyTask : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [MaxLength(200)]
        [Column("title")]
        public string Title { get; set; }

        [MaxLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        [Column("status")]
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;

        [Column("priority")]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("deadline")]
        public DateTime? Deadline { get; set; }

        [Required]
        [Column("project_id")]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<TaskAssignment> Assignments { get; set; } = [];

        public Guid GetId() => Id;
    }
}