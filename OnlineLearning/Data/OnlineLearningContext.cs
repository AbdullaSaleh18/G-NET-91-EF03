using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Entities;
using OnlineLearningPlatform.Configurations;

namespace OnlineLearningPlatform.Data
{
    public class OnlineLearningContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(OnlineLearningContext).Assembly);

            SeedData.Seed(modelBuilder);
        }
    

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=OnlineLearningDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

    }
}