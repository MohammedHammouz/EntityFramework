using ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Tables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // Look for files in current folder
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load appsettings.json
    .Build();
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("Connection string 'DefaultConnection' was not found.");
                return;
            }
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .LogTo(Console.WriteLine,LogLevel.Information)
                .EnableSensitiveDataLogging()
    .UseSqlServer(connectionString) // Tell EF Core to use SQL Server with this connection string
    .Options;
            using var context = new AppDBContext(options);
            Console.WriteLine("================================================");

            Console.WriteLine(context.Database.CanConnect()
                ? "Connected! You are ready to retrieve Data :-)"
                : "Failed to connect.");

            Console.WriteLine("================================================");
            PrintAndRetrieveData(context);
            GetActiveStudents(context);
            Example_First(context);
            Example_First(context);
            Example_Single(context);
            Example_SingleOrDefault(context);
        }
        public static void PrintAndRetrieveData(AppDBContext context)
        {
        //    var query = context.Students
        //        .OrderBy(s => s.StudentID);
        //    PrintGeneratedSql("Students", query.ToQueryString());
        //    Console.WriteLine("Preview SQL using ToQueryString():");
        //    Console.WriteLine("----------------------------------");
        //    Console.WriteLine(query.ToQueryString());
        //    Console.WriteLine();
        //    var students = query.ToList();

        //    Console.WriteLine(
        //$"Rows Returned: {students.Count}");
        //    Console.WriteLine();
        //    Console.WriteLine(new string('=', 70));
        //    Console.WriteLine();
        //    foreach (var student in students)
        //    {
        //        Console.WriteLine(
        //            $"Id: {student.StudentID}, " +
        //            $"Name: {student.FirstName} {student.LastName}, " +
        //            $"Email: {student.Email}, " +
        //            $"Status: {student.Status}, " +
        //            $"Phone: {student.PhoneNumber ?? "N/A"}");
        //    }
        //    var query1 = context.Enrollments.Where(e => e.Status == "Completed");
        //    var enrollments = query1.ToList();
        //    Console.WriteLine(query1.ToQueryString());
        //    var query2 = context.Instructors
        //        .Where(i => i.Salary > 5000)
        //        .Select(i => i.InstructorID);
        //    var isntructors = query2
        //        .ToList();
        //    Console.WriteLine(query2.ToQueryString());
            var query3 = context.Instructors.Where(i => i.IsActive==true);
            var instructors = query3.ToList();
            Console.WriteLine(query3.ToQueryString());
        }
        public static void GetActiveStudents(AppDBContext context)
        {
            var query = context.Students
                .Where(s => s.Status == "Active")
                .OrderBy(s=>s.StudentID);
            var students = query.ToList();
            PrintGeneratedSql("Students", query.ToQueryString());
            // Print results
            Console.WriteLine("\nActive Students:");
            Console.WriteLine("----------------");

            foreach (var student in students)
            {
                Console.WriteLine($"{student.StudentID} - {student.FirstName} {student.LastName}");
            }

            Console.WriteLine();
            Console.WriteLine($"Total Active Students: {students.Count}");

        }
        public static void Example_First(AppDBContext context)
        {
            Console.WriteLine("\nExample 1 - First()");
            Console.WriteLine("-------------------");
            // Build query first(no execution yet)
            var query = context.Students
                .Where(s => s.Status == "Active")
                .OrderBy(s => s.StudentID);
            // Preview query shape
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            // Runtime logging will show the actual executed SQL.
            var student = query.First();
            Console.WriteLine("\nFirst Active Student:");
            Console.WriteLine($"{student.StudentID} - {student.FirstName} {student.LastName}");
            
        }
            /// <summary>
            /// FirstOrDefault() returns the first matching row,
            /// or null if no row exists.
            /// </summary>
            public static void Example_FirstOrDefault(AppDBContext context)
            {
            Console.WriteLine("\nExample 2 - FirstOrDefault()");
            Console.WriteLine("----------------------------");

            // Build query first (no execution yet)
            var query = context.Students
                .Where(s => s.Email == "notfound@student.com");
            // Execute query
            // Runtime logging will show the actual executed SQL.
            var student = query.FirstOrDefault();

            if (student == null)
            {
                Console.WriteLine("\nNo student found.");
            }
            else
            {
                Console.WriteLine("\nStudent Found:");
                Console.WriteLine($"{student.StudentID} - {student.FirstName} {student.LastName}");
            }
        }
       public static void Example_Single(AppDBContext context)
        {
            Console.WriteLine("\nExample 3 - Single()");
            Console.WriteLine("--------------------");

            // Build query first (no execution yet)
            var query = context.Courses
                .Where(c => c.Code == "SQL-101");
            // Preview query shape
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            // Runtime logging will show the actual executed SQL.
            var course = query.Single();
            Console.WriteLine("\nCourse Found:");
            Console.WriteLine($"{course.CourseId} - {course.Code} - {course.Title}");
        }
        /// <summary>
        /// SingleOrDefault() expects zero or one matching row.
        /// Returns null if none exists, but throws if duplicates exist.
        /// </summary>
        public static void Example_SingleOrDefault(AppDBContext context)
        {
            Console.WriteLine("\nExample 4 - SingleOrDefault()");
            Console.WriteLine("-----------------------------");

            // Build query first (no execution yet)
            var query = context.Courses
                .Where(c => c.Code == "UNKNOWN-999");

            // Preview query shape
            PreviewSQLUsingToQueryString(query.ToQueryString());

            // Execute query
            // Runtime logging will show the actual executed SQL.
            var course = query.SingleOrDefault();

            if (course == null)
            {
                Console.WriteLine("\nNo course found.");
            }
            else
            {
                Console.WriteLine("\nCourse Found:");
                Console.WriteLine($"{course.CourseId} - {course.Code} - {course.Title}");
            }
        }
        public static void PrintGeneratedSql(string tableName,string sqlQuery)
        {
            Console.WriteLine($"Generated SQL Query for {tableName}:");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(sqlQuery);
            Console.WriteLine();
        }
        static void PreviewSQLUsingToQueryString(string SQLString)
        {
            Console.WriteLine("\nPreview SQL using ToQueryString():");
            Console.WriteLine("----------------------------------");
            Console.WriteLine(SQLString);
            Console.WriteLine();
        }
    }
}
