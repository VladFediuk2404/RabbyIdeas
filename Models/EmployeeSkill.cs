using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("employee_skills")]
    public class EmployeeSkill
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("employee_id")]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [MaxLength(50)]
        [Column("skill_name")]
        public string SkillName { get; set; }

        [Column("category")]
        public SkillCategory Category { get; set; }
    }
}
