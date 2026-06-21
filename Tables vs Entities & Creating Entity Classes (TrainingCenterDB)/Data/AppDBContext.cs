using ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Data
{
    public partial class AppDBContext:DbContext
    {
        // ------------------------------------------------------
        // Constructor
        // ------------------------------------------------------
        // Receives DbContextOptions from Program.cs
        // Example:
        // builder.Services.AddDbContext<AppDbContext>(options =>
        //      options.UseSqlServer(connectionString));
        // ------------------------------------------------------
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
        // ======================================================
        // DbSet Properties
        // ------------------------------------------------------
        // Each DbSet<T> represents a table in the database.
        // EF Core will query these tables.
        // ======================================================


        // Represents Courses table
        public virtual DbSet<Course> Courses { get; set; }


        // Represents Enrollments table
        public virtual DbSet<Enrollment> Enrollments { get; set; }


        // Represents Instructors table
        public virtual DbSet<Instructor> Instructors { get; set; }


        // Represents Students table
        public virtual DbSet<Student> Students { get; set; }


        // Represents StudentProfiles table
        public virtual DbSet<StudentProfile> StudentProfiles { get; set; }
        // ======================================================
        // OnModelCreating
        // ------------------------------------------------------
        // This method is where we configure tables manually
        // using Fluent API.
        //
        // Why needed?
        // Because not everything can be guessed by conventions.
        //
        // Here we configure:
        // 🔹 Keys
        // 🔹 Relationships
        // 🔹 Indexes
        // 🔹 Column types
        // 🔹 Default values
        // 🔹 Max lengths
        // ======================================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ==================================================
            // Configure Course Entity
            // ==================================================
            modelBuilder.Entity<Course>(entity =>
            {


                entity.HasKey(e => e.CourseId);
                // Create index on InstructorId
                // Speeds searching courses by instructor
                entity.HasIndex(e => e.InstructorId, "IX_Courses_InstructorId");


                // Create index on Status
                entity.HasIndex(e => e.Status, "IX_Courses_Status");


                // Course Code must be unique
                // Example: C#101 cannot repeat
                entity.HasIndex(e => e.Code, "UQ_Courses_Code")
                      .IsUnique();


                // Maximum 30 characters
                entity.Property(e => e.Code)
                      .HasMaxLength(30);


                // Default value = current date/time
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");


                // Optional description max 500 chars
                entity.Property(e => e.Description)
                      .HasMaxLength(500);


                // Beginner / Intermediate / Advanced
                entity.Property(e => e.Level)
                      .HasMaxLength(30);


                // Decimal with precision (10,2)
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(10, 2)");


                // Publish date
                entity.Property(e => e.PublishedAt)
                      .HasColumnType("datetime");


                // Active / Draft / Closed
                entity.Property(e => e.Status)
                      .HasMaxLength(20);


                // Course title max 150
                entity.Property(e => e.Title)
                      .HasMaxLength(150);


                // Relationship:
                // One Instructor has many Courses
                entity.HasOne(d => d.Instructor)
                      .WithMany(p => p.Courses)
                      .HasForeignKey(d => d.InstructorId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Courses_Instructors");
            });
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);
                entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");
                entity.HasIndex(e => e.Status, "IX_Enrollments_Status");
                entity.HasIndex(e => e.StudentId, "IX_Enrollments_StudentId");
                entity.HasIndex(e => new { e.StudentId, e.CourseId }, "UQ_Enrollments_StudentId_CourseId").IsUnique();
                entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.CompletionDate).HasColumnType("datetime");
                entity.Property(e => e.ProgressPercent).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.FinalGrade).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.HasOne(d=>d.Student)
                .WithMany(d=>d.Enrolments)
                .HasForeignKey(d=>d.StudentId)
                .HasConstraintName("FK_Enrollments_Students");
                entity.HasOne(d=>d.Course)
                .WithMany(d=>d.Enrollments)
                .HasForeignKey(d=>d.CourseId)
                .HasConstraintName("FK_Enrollments_Courses");
            });
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(e => e.InstructorID);
                entity.HasIndex(e => e.ManagerID, "IX_Instructors_ManagerId");
                entity.HasIndex(e => e.Email, "UQ_Instructors_Email").IsUnique();
                //entity.HasKey(e=>e.)
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.HireDate).HasColumnType("date");
                entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.HasOne(d => d.Manager)
                .WithMany(d => d.InverseManager)
                .HasForeignKey(d => d.ManagerID)
                .HasConstraintName("FK_Instructors_Manager");
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentID);
                entity.HasIndex(e => e.Status, "IX_Students_Status");


                // Email unique
                entity.HasIndex(e => e.Email, "UQ_Students_Email")
                      .IsUnique();


                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(30);


                // Registration date defaults to now
                entity.Property(e => e.RegisteredAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");


                entity.Property(e => e.Status).HasMaxLength(20);
            });
            modelBuilder.Entity<StudentProfile>(entity =>
            {
                // Primary key = StudentId
                entity.HasKey(e => e.StudentId);


                // Not auto increment
                entity.Property(e => e.StudentId)
                      .ValueGeneratedNever();


                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Bio).HasMaxLength(500);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.LinkedInUrl).HasMaxLength(200);


                // One Student has one Profile
                entity.HasOne(d => d.Student)
                      .WithOne(p => p.StudentProfile)
                      .HasForeignKey<StudentProfile>(d => d.StudentId)
                      .HasConstraintName("FK_StudentProfiles_Students");
            });


            // Optional extension point
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
