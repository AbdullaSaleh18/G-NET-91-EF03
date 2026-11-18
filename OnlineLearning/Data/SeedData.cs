using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Entities;
using OnlineLearningPlatform.Enums;

namespace OnlineLearningPlatform.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor
                {
                    Id = 1,
                    FullName = "Ahmed Mohamed",
                    Email = "ahmed@example.com",
                    Bio = "Programming Instructor",
                    JoinedDate = new DateTime(2025, 1, 1)
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Programming" },
                new Category { Id = 2, Name = "Backend", ParentCategoryId = 1 },
                new Category { Id = 3, Name = "Frontend", ParentCategoryId = 1 }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Title = "C# Fundamentals",
                    Description = "Learn C# basics",
                    Price = 500,
                    Level = CourseLevel.Beginner,
                    IsPublished = true,
                    PublishedDate = new DateTime(2025, 2, 1),
                    InstructorId = 1,
                    CategoryId = 2
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FullName = "Omar Ali",
                    Email = "omar@example.com",
                    DateOfBirth = new DateTime(2002, 5, 10)
                },
                new Student
                {
                    Id = 2,
                    FullName = "Sara Ahmed",
                    Email = "sara@example.com",
                    DateOfBirth = new DateTime(2001, 8, 15)
                }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    StudentId = 1,
                    CourseId = 1,
                    EnrollmentDate = new DateTime(2025, 3, 1),
                    ProgressPercentage = 50,
                    Status = EnrollmentStatus.Active,
                    FinalGrade = null
                }
            );
        }
    }
}