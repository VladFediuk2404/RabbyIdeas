using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("client_reports")]
    public class ClientReport : IIdentifiable
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [Column("client_id")]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        [Required]
        [MaxLength(2000)]
        [Column("requirements")]
        public string Requirements { get; set; }

        [MaxLength(2000)]
        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("status")]
        public ReportStatus Status { get; set; } = ReportStatus.New;

        [Required]
        [Column("manager_id")]
        public Guid ManagerId { get; set; }
        public Employee Manager { get; set; }

        [Column("project_id")]
        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }

        public Guid GetId() => Id;
    }
}