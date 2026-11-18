using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Entities;

namespace OnlineLearningPlatform.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Description)
                   .HasMaxLength(1000);

            builder.Property(c => c.Price)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(c => c.Level)
                   .IsRequired();

            builder.Property(c => c.IsPublished)
                   .IsRequired();

            builder.Property(c => c.PublishedDate)
                   .IsRequired(false);

            builder.Property(c => c.InstructorId)
                   .IsRequired();

            builder.Property(c => c.CategoryId)
                   .IsRequired();

            builder.HasOne<Instructor>()
                   .WithMany()
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Category>()
                   .WithMany()
                   .HasForeignKey(c => c.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}