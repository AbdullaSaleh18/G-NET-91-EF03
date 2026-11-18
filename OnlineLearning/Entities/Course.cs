using OnlineLearningPlatform.Enums;
namespace OnlineLearningPlatform.Entities
{
    public class Course
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public CourseLevel Level { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedDate { get; set; }

        public int InstructorId { get; set; }

        public int CategoryId { get; set; }
    }
}