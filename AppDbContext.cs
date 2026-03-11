using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientReport> ClientReports { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectAppointment> ProjectAppointments { get; set; }
        public DbSet<MyTask> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<TaskCommit> TaskCommits { get; set; }
        public DbSet<TaskDeliverable> TaskDeliverables { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity таблицы → snake_case
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");

            // User → Company
            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Members)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Company → Owner
            modelBuilder.Entity<Company>()
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee → User
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Employee → Company
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee → Skills
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Skills)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ClientReport → Client
            modelBuilder.Entity<ClientReport>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reports)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClientReport → Manager
            modelBuilder.Entity<ClientReport>()
                .HasOne(r => r.Manager)
                .WithMany()
                .HasForeignKey(r => r.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClientReport → Project
            modelBuilder.Entity<ClientReport>()
                .HasOne(r => r.Project)
                .WithOne(p => p.ClientReport)
                .HasForeignKey<ClientReport>(r => r.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            // Project → Company
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Company)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project → Manager
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany()
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project → ScrumMaster
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ScrumMaster)
                .WithMany()
                .HasForeignKey(p => p.ScrumMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProjectMember
            modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.Employee)
            .WithMany(e => e.Projects)
            .HasForeignKey(pm => pm.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Employee)
                .WithMany()
                .HasForeignKey(pm => pm.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProjectAppointment → ScrumMaster
            modelBuilder.Entity<ProjectAppointment>()
                .HasOne(pa => pa.ScrumMaster)
                .WithMany()
                .HasForeignKey(pa => pa.ScrumMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProjectAppointment → AppointedBy
            modelBuilder.Entity<ProjectAppointment>()
                .HasOne(pa => pa.AppointedBy)
                .WithMany()
                .HasForeignKey(pa => pa.AppointedById)
                .OnDelete(DeleteBehavior.Restrict);

            // TaskAssignment → Employee
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Employee)
                .WithMany(e => e.TaskAssignments)
                .HasForeignKey(ta => ta.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // TaskAssignment → AssignedBy
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.AssignedBy)
                .WithMany()
                .HasForeignKey(ta => ta.AssignedById)
                .OnDelete(DeleteBehavior.Restrict);

            // TaskCommit → Employee
            modelBuilder.Entity<TaskCommit>()
                .HasOne(tc => tc.Employee)
                .WithMany()
                .HasForeignKey(tc => tc.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // TaskDeliverable → Employee
            modelBuilder.Entity<TaskDeliverable>()
                .HasOne(td => td.Employee)
                .WithMany()
                .HasForeignKey(td => td.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment → ParentComment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            //
            // Comment → Employee
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Employee)
                .WithMany()
                .HasForeignKey(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
