using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("task_assignments")]
    public class TaskAssignment : IIdentifiable
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

        [Column("assigned_at")]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        [Column("assigned_by")]
        public Guid AssignedById { get; set; }
        public Employee AssignedBy { get; set; }

        public Guid GetId()
        {
            throw new NotImplementedException();
        }
    }
}
