using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Data;
using OnlineLearningPlatform.Entities;
using OnlineLearningPlatform.Enums;

using var db = new OnlineLearningContext();

//// CREATE
//AddInstructor(db);
//AddCourse(db);
//AddStudent(db);
//AddEnrollment(db);

//// READ
//GetAllCourses(db);
//GetAllStudents(db);
//GetAllInstructors(db);
//GetAllEnrollments(db);
//FindCourseById(db);
//FilterCoursesByPrice(db);
//GetSubcategories(db);
//GetCourseRoster(db);

//// UPDATE
//UpdateCoursePrice(db);
//UpdateEnrollmentProgress(db);
//PublishCourse(db);

//// DELETE
//UnenrollStudent(db);
//DeleteCategory(db);
//DeleteCourseWithEnrollments(db);


// ---------------- CREATE ----------------

static void AddInstructor(OnlineLearningContext db)
{
    var instructor = new Instructor
    {
        FullName = "Mohamed Hassan",
        Email = "mohamed@example.com",
        Bio = "C# and .NET Instructor",
        JoinedDate = DateTime.Now
    };

    db.Instructors.Add(instructor);
    db.SaveChanges();

    Console.WriteLine($"Instructor added successfully. ID: {instructor.Id}");
}

static void AddCourse(OnlineLearningContext db)
{
    var instructor = db.Instructors
        .OrderByDescending(i => i.Id)
        .FirstOrDefault();

    var category = db.Categories
        .Where(c => c.ParentCategoryId != null)
        .OrderBy(c => c.Id)
        .FirstOrDefault();

    if (instructor == null || category == null)
    {
        Console.WriteLine("Instructor or Category not found.");
        return;
    }

    var course = new Course
    {
        Title = "ASP.NET Core",
        Description = "Learn ASP.NET Core",
        Price = 700,
        Level = CourseLevel.Intermediate,
        IsPublished = true,
        PublishedDate = DateTime.Now,
        InstructorId = instructor.Id,
        CategoryId = category.Id
    };

    db.Courses.Add(course);
    db.SaveChanges();

    Console.WriteLine($"Course added successfully. ID: {course.Id}");
}

static void AddStudent(OnlineLearningContext db)
{
    var students = new List<Student>
    {
        new Student
        {
            FullName = "Ali Hassan",
            Email = "ali@example.com",
            DateOfBirth = new DateTime(2003, 5, 15)
        },
        new Student
        {
            FullName = "Mona Ahmed",
            Email = "mona@example.com",
            DateOfBirth = new DateTime(2002, 7, 20)
        },
        new Student
        {
            FullName = "Khaled Mohamed",
            Email = "khaled@example.com",
            DateOfBirth = new DateTime(2001, 3, 10)
        }
    };

    db.Students.AddRange(students);
    db.SaveChanges();

    foreach (var student in students)
    {
        Console.WriteLine($"Student added successfully. ID: {student.Id}");
    }
}

static void AddEnrollment(OnlineLearningContext db)
{
    var student = db.Students
        .OrderByDescending(s => s.Id)
        .FirstOrDefault();

    var course = db.Courses
        .OrderByDescending(c => c.Id)
        .FirstOrDefault();

    if (student == null)
    {
        Console.WriteLine("Student not found.");
        return;
    }

    if (course == null)
    {
        Console.WriteLine("Course not found.");
        return;
    }

    if (!course.IsPublished)
    {
        Console.WriteLine("Cannot enroll. Course is not published.");
        return;
    }

    bool alreadyEnrolled = db.Enrollments.Any(e =>
        e.StudentId == student.Id && e.CourseId == course.Id);

    if (alreadyEnrolled)
    {
        Console.WriteLine("Student is already enrolled in this course.");
        return;
    }

    var enrollment = new Enrollment
    {
        StudentId = student.Id,
        CourseId = course.Id,
        EnrollmentDate = DateTime.Now,
        ProgressPercentage = 0,
        Status = EnrollmentStatus.Active,
        FinalGrade = null
    };

    db.Enrollments.Add(enrollment);
    db.SaveChanges();

    Console.WriteLine("Student enrolled successfully.");
}


// ---------------- READ ----------------

static void GetAllCourses(OnlineLearningContext db)
{
    var courses = db.Courses.ToList();

    foreach (var course in courses)
    {
        Console.WriteLine($"ID: {course.Id}");
        Console.WriteLine($"Title: {course.Title}");
        Console.WriteLine($"Price: {course.Price}");
        Console.WriteLine($"Level: {course.Level}");
        Console.WriteLine("----------------------");
    }
}

static void GetAllStudents(OnlineLearningContext db)
{
    var students = db.Students.ToList();

    foreach (var student in students)
    {
        Console.WriteLine($"ID: {student.Id}");
        Console.WriteLine($"Name: {student.FullName}");
        Console.WriteLine($"Email: {student.Email}");
        Console.WriteLine("----------------------");
    }
}

static void GetAllInstructors(OnlineLearningContext db)
{
    var instructors = db.Instructors.ToList();

    foreach (var instructor in instructors)
    {
        Console.WriteLine($"ID: {instructor.Id}");
        Console.WriteLine($"Name: {instructor.FullName}");
        Console.WriteLine($"Email: {instructor.Email}");
        Console.WriteLine("----------------------");
    }
}

static void GetAllEnrollments(OnlineLearningContext db)
{
    var enrollments = db.Enrollments.ToList();

    foreach (var enrollment in enrollments)
    {
        Console.WriteLine($"Student ID: {enrollment.StudentId}");
        Console.WriteLine($"Course ID: {enrollment.CourseId}");
        Console.WriteLine($"Enrollment Date: {enrollment.EnrollmentDate}");
        Console.WriteLine($"Progress: {enrollment.ProgressPercentage}%");
        Console.WriteLine($"Status: {enrollment.Status}");
        Console.WriteLine("----------------------");
    }
}

static void FindCourseById(OnlineLearningContext db)
{
    int courseId = db.Courses
        .OrderBy(c => c.Id)
        .Select(c => c.Id)
        .FirstOrDefault();

    if (courseId == 0)
    {
        Console.WriteLine("No courses found.");
        return;
    }

    var courseById = db.Courses.Find(courseId);

    if (courseById == null)
    {
        Console.WriteLine("Course not found.");
    }
    else
    {
        Console.WriteLine($"Found by ID: {courseById.Id} - {courseById.Title}");
    }

    string title = courseById?.Title ?? "";

    var courseByTitle = db.Courses.FirstOrDefault(c => c.Title == title);

    if (courseByTitle == null)
    {
        Console.WriteLine("Course not found.");
    }
    else
    {
        Console.WriteLine($"Found by Title: {courseByTitle.Title}");
    }
}

static void FilterCoursesByPrice(OnlineLearningContext db)
{
    decimal minPrice = 400;
    decimal maxPrice = 800;

    var courses = db.Courses
        .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
        .OrderByDescending(c => c.Price)
        .ToList();

    foreach (var course in courses)
    {
        Console.WriteLine($"Title: {course.Title}");
        Console.WriteLine($"Level: {course.Level}");
        Console.WriteLine($"Price: {course.Price}");
        Console.WriteLine("----------------------");
    }
}

static void GetSubcategories(OnlineLearningContext db)
{
    int categoryId = 1;

    var subcategories = db.Categories
        .Where(c => c.ParentCategoryId == categoryId)
        .ToList();

    Console.WriteLine("Subcategories:");

    foreach (var category in subcategories)
    {
        Console.WriteLine($"ID: {category.Id} - Name: {category.Name}");
    }

    var topLevelCategories = db.Categories
        .Where(c => c.ParentCategoryId == null)
        .ToList();

    Console.WriteLine("Top-Level Categories:");

    foreach (var category in topLevelCategories)
    {
        Console.WriteLine($"ID: {category.Id} - Name: {category.Name}");
    }
}

static void GetCourseRoster(OnlineLearningContext db)
{
    int courseId = db.Enrollments
        .Select(e => e.CourseId)
        .FirstOrDefault();

    if (courseId == 0)
    {
        Console.WriteLine("No enrollments found.");
        return;
    }

    var studentIds = db.Enrollments
        .Where(e => e.CourseId == courseId)
        .Select(e => e.StudentId)
        .ToList();

    var students = db.Students
        .Where(s => studentIds.Contains(s.Id))
        .ToList();

    Console.WriteLine($"Number of enrolled students: {students.Count}");

    foreach (var student in students)
    {
        Console.WriteLine($"Student ID: {student.Id} - Name: {student.FullName}");
    }
}


// ---------------- UPDATE ----------------

static void UpdateCoursePrice(OnlineLearningContext db)
{
    var course = db.Courses
        .OrderByDescending(c => c.Id)
        .FirstOrDefault();

    if (course == null)
    {
        Console.WriteLine("Course not found.");
        return;
    }

    course.Price = 600;
    db.SaveChanges();

    Console.WriteLine($"Course price updated successfully. New Price: {course.Price}");
}

static void UpdateEnrollmentProgress(OnlineLearningContext db)
{
    var enrollment = db.Enrollments
        .OrderByDescending(e => e.EnrollmentDate)
        .FirstOrDefault();

    decimal newProgress = 100;

    if (newProgress < 0 || newProgress > 100)
    {
        Console.WriteLine("Progress must be between 0 and 100.");
        return;
    }

    if (enrollment == null)
    {
        Console.WriteLine("Enrollment not found.");
        return;
    }

    enrollment.ProgressPercentage = newProgress;

    if (newProgress == 100)
    {
        enrollment.Status = EnrollmentStatus.Completed;
    }

    db.SaveChanges();

    Console.WriteLine("Enrollment progress updated successfully.");
    Console.WriteLine($"Progress: {enrollment.ProgressPercentage}%");
    Console.WriteLine($"Status: {enrollment.Status}");
}

static void PublishCourse(OnlineLearningContext db)
{
    var course = db.Courses
        .OrderByDescending(c => c.Id)
        .FirstOrDefault();

    if (course == null)
    {
        Console.WriteLine("Course not found.");
        return;
    }

    course.IsPublished = true;
    course.PublishedDate = DateTime.Now;

    db.SaveChanges();

    Console.WriteLine($"Course '{course.Title}' published successfully.");
    Console.WriteLine($"Published Date: {course.PublishedDate}");
}


// ---------------- DELETE ----------------

static void UnenrollStudent(OnlineLearningContext db)
{
    var enrollment = db.Enrollments
        .OrderByDescending(e => e.EnrollmentDate)
        .FirstOrDefault();

    if (enrollment == null)
    {
        Console.WriteLine("Enrollment not found.");
        return;
    }

    int studentId = enrollment.StudentId;
    int courseId = enrollment.CourseId;

    var enrollmentToDelete = db.Enrollments.Find(studentId, courseId);

    if (enrollmentToDelete == null)
    {
        Console.WriteLine("Enrollment not found.");
        return;
    }

    db.Enrollments.Remove(enrollmentToDelete);
    db.SaveChanges();

    Console.WriteLine("Student unenrolled successfully.");
}

static void DeleteCategory(OnlineLearningContext db)
{
    int categoryId = 1;

    var category = db.Categories.Find(categoryId);

    if (category == null)
    {
        Console.WriteLine("Category not found.");
        return;
    }

    // First attempt: try deleting the category directly.
    db.Categories.Remove(category);

    try
    {
        db.SaveChanges();
        Console.WriteLine("Category deleted successfully.");
    }
    catch (DbUpdateException)
    {
        Console.WriteLine("Cannot delete category because it has related records.");

        db.Entry(category).State = EntityState.Unchanged;
    }

    // Safe delete: check for subcategories before deleting.
    bool hasSubcategories = db.Categories
        .Any(c => c.ParentCategoryId == categoryId);

    if (hasSubcategories)
    {
        Console.WriteLine("Safe delete refused: Category has subcategories.");
        return;
    }

    // Also avoid deleting a category that is still used by courses.
    bool hasCourses = db.Courses.Any(c => c.CategoryId == categoryId);

    if (hasCourses)
    {
        Console.WriteLine("Safe delete refused: Category is used by courses.");
        return;
    }

    db.Categories.Remove(category);
    db.SaveChanges();

    Console.WriteLine("Category deleted successfully.");
}

static void DeleteCourseWithEnrollments(OnlineLearningContext db)
{
    var course = db.Courses
        .OrderByDescending(c => c.Id)
        .FirstOrDefault();

    if (course == null)
    {
        Console.WriteLine("Course not found.");
        return;
    }

    bool hasEnrollments = db.Enrollments
        .Any(e => e.CourseId == course.Id);

    if (!hasEnrollments)
    {
        var student = db.Students
            .OrderByDescending(s => s.Id)
            .FirstOrDefault();

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        db.Enrollments.Add(new Enrollment
        {
            StudentId = student.Id,
            CourseId = course.Id,
            EnrollmentDate = DateTime.Now,
            ProgressPercentage = 0,
            Status = EnrollmentStatus.Active,
            FinalGrade = null
        });

        db.SaveChanges();
    }

    int beforeCount = db.Enrollments.Count(e => e.CourseId == course.Id);

    Console.WriteLine($"Enrollments before deletion: {beforeCount}");

    // Enrollment records are deleted automatically because the Course relationship uses Cascade delete.
    db.Courses.Remove(course);
    db.SaveChanges();

    int afterCount = db.Enrollments.Count(e => e.CourseId == course.Id);

    Console.WriteLine($"Enrollments after deletion: {afterCount}");
    Console.WriteLine("Course and its enrollments deleted successfully.");
}
